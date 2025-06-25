using System.ComponentModel.DataAnnotations;

namespace Paup.Models;

public class DodatniTrosak
{
    [Required(ErrorMessage = "Unesite naslov troška.")]
    [StringLength(30, ErrorMessage = "Naslov može imati najviše 30 znakova.")]

    public string Naslov { get; set; }

    [Required(ErrorMessage = "Unesite opis troška.")]
    [StringLength(250, ErrorMessage = "Naslov može imati najviše 250 znakova.")]

    public string Opis { get; set; }

    [Range(0, 10000, ErrorMessage = "Unesite valjanu cijenu.")]
    public double Cijena { get; set; }
}