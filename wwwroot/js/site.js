// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// function validateForm(){
    
//     console.log("1");
//     // Validate fields
//     let isValid = true;

//     const username = document.getElementById('email').value.trim();
//     const password = document.getElementById('passwordinput').value.trim();
//     console.log("2");
//     if (username === '') {
//         document.getElementById('emailError').classList.remove('opacity-0');
//         isValid = false;
//     }

//     if (password === '') {
//         document.getElementById('passwordError').classList.remove('opacity-0');
//         isValid = false;
//     }
    
//     if (isValid) {
//         // Submit the form if all fields are valid
        
//         window.location.href = "superadmindashboard.html";
//         this.submit();
//     }
   
// }
document.getElementById('login-form').addEventListener('submit', function(e) {
    console.log("1");
    // Validate fields
    
    let isValid = true;

    const email = document.getElementById('email').value.trim();
    const password = document.getElementById('passwordinput').value.trim();
    if (email === '') {
        console.log("2");
        document.getElementById('emailError').classList.remove('opacity-0');
        // document.getElementById('emailError').textContent='Email is Required';
        isValid = false;
    }
    if (password === '') {
        document.getElementById('passwordError').classList.remove('opacity-0');
        // document.getElementById('passwordError').classList.add('opacity-1');
        isValid = false;
    }
    
    if (isValid) {
        
        window.location.href='superadmindashboard.html';
        console.log("3")
        // e.preventDefault()
    }
    else{
        e.preventDefault();

    }

});

document.getElementById('email').addEventListener('input', function() {
    document.getElementById('emailError').classList.add('opacity-0');
});

document.getElementById('passwordinput').addEventListener('input', function() {
    document.getElementById('passwordError').classList.add('opacity-0');
});


function changevisibility() {
    var x = document.getElementById("passwordinput");
    if (x.type === "password") {
      x.type = "text";
      var y = document.getElementById("seepassword");
      y.classList.remove('d-none')
      y.classList.add('d-block')
      var z = document.getElementById("hidepassword");
      z.classList.remove('d-block')
      z.classList.add('d-none')

      
    } else {
      x.type = "password";
      var y = document.getElementById("hidepassword");
      y.classList.remove('d-none')
      y.classList.add('d-block')
      var z = document.getElementById("seepassword");
      z.classList.remove('d-block')
      z.classList.add('d-none')
    }
  }


