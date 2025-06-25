using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Paup.Models;

public enum VrstaPonude { Prodaja, Najam}
public enum MjeraKolicine { Komad, m2 }
public enum NajamTip { Dan, m2PoDanu }

public class Oprema
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ime je obavezno.")]
    [StringLength(30, ErrorMessage = "Ime može imati najviše 30 znakova.")]
    [DisplayName("Naslov")]
    public string Ime { get; set; }
    
    [Required(ErrorMessage = "Opis je obavezan.")]
    [StringLength(200, ErrorMessage = "Opis može imati najviše 200 znakova.")]
    public string Opis { get; set; }

    [Required(ErrorMessage = "Kolicina je obavezna.")]
    [Range(1, 999, ErrorMessage = "Količina mora biti između 1 i 999.")]
    public int Kolicina { get; set; }

    public MjeraKolicine Mjera { get; set; }
    
    public VrstaPonude VrstaPonude { get; set; } = VrstaPonude.Prodaja;
    
    [Required(ErrorMessage = "Cijena je obavezna.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Unesite ispravnu cijenu.")]
    public decimal Cijena { get; set; }
    public NajamTip? NajamPo { get; set; }
    
    public decimal? CijenaNajma { get; set; }

    [BindNever]
    public string? PruzateljUslugeId { get; set; }
    public AppKorisnik? PruzateljUsluge { get; set; }
}