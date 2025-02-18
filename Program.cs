using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pizzashop.Models;
using PizzaShop.Services;

var builder = WebApplication.CreateBuilder(args);
var jwtConfig = builder.Configuration.GetSection("jwt");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Distributed Memory Cache (required for session)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<JwtServices>();

// JWT service - Adding JWT Authentication
builder.Services.AddAuthentication(x=>{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["issuer"],  // The issuer of the token (e.g., your app's URL)
            ValidAudience = jwtConfig["audience"], // The audience for the token (e.g., your API)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["key"] ?? "")), // The key to validate the JWT's signature
            // RoleClaimType = ClaimTypes.Role,
            // NameClaimType = ClaimTypes.Name 
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Check for the token in cookies
                var token = context.Request.Cookies["jwt"]; // Change "AuthToken" to your cookie name if it's different
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers["Authorization"] = "Bearer " + token;
                }
                return Task.CompletedTask;
            }
        };
    }
);
// Authorization settings
// builder.Services.AddAuthorization(options =>
// {
//     // Add authorization policies
//     options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
// });

// Scoped services and DB Context
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddDbContext<PizzashopMainContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Ensure authentication and authorization come before routing
app.UseRouting();

// Apply session and authentication middleware
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Define the default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}"
);

app.Run();







// using System.Text;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using pizzashop.Models;


// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddControllersWithViews();
// builder.Services.AddSession(options =>
// {
//     options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
//     options.Cookie.HttpOnly = true;
//     options.Cookie.IsEssential = true;
// });

// // jwt service

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         // Set the options for JWT bearer authentication
//         Console.WriteLine("inside jwt bearer");
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = "yourdomain.com",
//             ValidAudience = "yourdomain.com",
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_super_secret_key"))
//         };

//         options.Events = new JwtBearerEvents
//         {
//             OnMessageReceived = context =>
//             {
//                 var token = context.HttpContext.Request.Cookies["jwt"];
//                 if (!string.IsNullOrEmpty(token))
//                 {
//                     context.Token = token;
//                     Console.WriteLine(" context Token: " + token);
//                 }
//                 return Task.CompletedTask;
//             }
//         };
//     });
// // Add distributed memory cache (required for session)
// builder.Services.AddDistributedMemoryCache();

// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options =>
//     {
//         options.Cookie.Name = "jwt";
//         options.Cookie.HttpOnly = true;
//         // options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
//         options.SlidingExpiration = true;
//         options.LoginPath = "/User/Login"; // Path to redirect if not authenticated
//     });

    

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
// });

// builder.Services.AddScoped<IEmailService, EmailService>();


// builder.Services.AddDbContext<PizzashopMainContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();
// app.UseSession();
// app.UseAuthentication();
// app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=User}/{action=Login}/{id?}");
    
// app.Run();