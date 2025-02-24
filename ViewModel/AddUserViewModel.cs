namespace pizzashop.ViewModel;

public partial class AddUserViewModel
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string UserName { get; set; } = null!;

    public string? Phone { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Status { get; set; }

    public string? Country { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public string? Address { get; set; }

    public string? Zipcode { get; set; }

    public string? RoleName { get; set; }

    public string? Profile { get; set; }

    public Guid Createdby { get; set; }


}
