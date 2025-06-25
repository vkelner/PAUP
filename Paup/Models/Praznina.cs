using System.ComponentModel.DataAnnotations;

namespace Paup.Models;

public class Praznina
{
    [Required(ErrorMessage = "Unesite visinu praznine.")]
    [Range(0.1, 100, ErrorMessage = "Visina praznine mora biti između 0.1 i 100 metara.")]
    public double Sirina { get; set; }
    [Required(ErrorMessage = "Unesite visinu praznine.")]
    [Range(0.1, 100, ErrorMessage = "Visina praznine mora biti između 0.1 i 100 metara.")]
    public double Visina { get; set; }
    
    [StringLength(50, ErrorMessage = "Naslov može imati najviše 20 znakova.")]
    public string Tip { get; set; }
}