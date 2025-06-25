using System.ComponentModel.DataAnnotations;
using Paup.Enum;


namespace Paup.Models;

public class RegisterView
{
    [Required(ErrorMessage = "Unesi korisničko ime!")]
    [Display(Name = "Korisničko ime")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Unesi email adresu!")]
    [EmailAddress(ErrorMessage = "Unesite ispravnu email adresu")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Kontakt broj je obavezan.")]
    [RegularExpression(@"^\+?[0-9]{8,15}$", ErrorMessage = "Unesite ispravan broj telefona.")]
    [Display(Name = "Kontakt broj")]
    public string KontaktBroj { get; set; }
    
    [Display(Name = "Unesi zaporku")]
    [Required(ErrorMessage = "Unesi lozinku!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Display(Name = "Potvrdi zaporku")]
    [Required(ErrorMessage = "Unesi lozinku!")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Lozinke se ne podudaraju!")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Odaberi ulogu!")]
    [Display(Name = "Uloga")]
    public VrstaKorisnika OdabranaUloga { get; set; }
    
    [Display(Name = "Županija")]
    public Zupanija? Zupanija { get; set; }
    
    [Display(Name = "Županije")]
    public List<Zupanija>? Zupanije { get; set; }
}