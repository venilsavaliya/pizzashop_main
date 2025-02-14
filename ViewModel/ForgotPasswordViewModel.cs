using System.ComponentModel.DataAnnotations;

namespace pizzashop.ViewModel;
public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
