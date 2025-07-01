using System.ComponentModel.DataAnnotations;
using Paup.Enum;


namespace Paup.Models;

public class RegisterView
{
    [Required(ErrorMessage = "Unesi korisničko ime!")]
    [Display(Name = "Korisničko ime")]
    [RegularExpression(@"^(?!\s*$)[a-zA-ZčćžšđČĆŽŠĐ0-9\s]{1,40}$", ErrorMessage = "Korisničko ime može sadržavati samo slova, brojeve i razmake i ne smije biti prazno.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Unesi email adresu!")]
    [EmailAddress(ErrorMessage = "Unesite ispravnu email adresu")]
    [RegularExpression(@"\S+", ErrorMessage = "Email ne može sadržavati razmake.")]

    public string Email { get; set; }
    
    [Required(ErrorMessage = "Kontakt broj je obavezan.")]
    [RegularExpression(@"^\+?[0-9]{8,15}$", ErrorMessage = "Unesite ispravan broj telefona.")]
    [Display(Name = "Kontakt broj")]
    public string KontaktBroj { get; set; }
    
    [Required(ErrorMessage = "Unesi lozinku!")]
    [StringLength(100, MinimumLength = 10, ErrorMessage = "Lozinka mora imati najmanje 10 znakova.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\w\d\s]).+$",
        ErrorMessage = "Lozinka mora sadržavati veliko slovo, broj i poseban znak.")]
    [DataType(DataType.Password)]
    [Display(Name = "Unesi zaporku")]
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