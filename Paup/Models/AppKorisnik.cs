using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Paup.Models;

public class AppKorisnik : IdentityUser
{
    public string? Uloga { get; set; }

    public string? Zupanije { get; set; }
    
    [Required]
    public string KontaktBroj { get; set; }
}