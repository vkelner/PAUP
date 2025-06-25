using System.ComponentModel.DataAnnotations;

namespace Paup.Models;

public class Zid
{

    public int Id { get; set; }
    
    [Required(ErrorMessage = "Unesite visinu.")]
    [Range(0.5, 100, ErrorMessage = "Širina mora biti između 0.5 i 100 metara.")]
    public double Visina { get; set; }
    
    [Required(ErrorMessage = "Unesite visinu.")]
    [Range(0.5, 100, ErrorMessage = "Širina mora biti između 0.5 i 100 metara.")]
    public double Sirina { get; set; }
    
    public double Povrsina => Sirina * Visina;
    public List<Praznina> Praznine { get; set; } = new();

    public double PovrsinaZida => Visina * Sirina;

    public double PovrsinaPraznina => Praznine?.Sum(p => p.Sirina * p.Visina) ?? 0;

    public double EfektivnaPovrsina => Math.Max(0, PovrsinaZida - PovrsinaPraznina);
    
}