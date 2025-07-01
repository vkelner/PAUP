using System.ComponentModel.DataAnnotations;
using Paup.Enum;
namespace Paup.Models;


public class LoginView
{
    [Required(ErrorMessage = "Unesite email adresu!")]
    [EmailAddress(ErrorMessage = "Unesite ispravnu email adresu")]
    [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Email ne može biti samo razmaci.")]

    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Unesite lozinku!")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}