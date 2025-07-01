using System.ComponentModel.DataAnnotations;
using Paup.Enum;


namespace Paup.Models
{
    public class Kalkulator
    {
        public VrstaMaterijala VrstaMaterijala { get; set; }
        
        [Display(Name = "Debljina")]
        [Required(ErrorMessage = "Unesite debljinu izolacije.")]
        [Range(2, 25, ErrorMessage = "Debljina mora biti između 2 i 25 cm.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Unesi cijeli broj.")]
        public int Debljina { get; set; }
        
        
        [Display(Name = "Broj zidova")]
        [Required(ErrorMessage = "Unesite broj zidova.")]
        [Range(1, 40, ErrorMessage = "Broj zidova mora biti između 1 i 40.")]
        public int BrojZidova { get; set; }
        public List<Zid> BrojZidovaLista { get; set; } = new();
        public List<DodatniTrosak> DodatniTroskovi { get; set; } = new();

        public double UkupnaPovrsina =>
            BrojZidovaLista?.Sum(z => z.EfektivnaPovrsina) ?? 0;

        public double CijenaPoM2 =>
            VrstaMaterijala switch
            {
                VrstaMaterijala.EPS => Math.Round(2.0 * Debljina, 2),
                VrstaMaterijala.Stiropor => Math.Round(1.0 * Debljina, 2),
                VrstaMaterijala.StaklenaVuna => Math.Round(1.5 * Debljina, 2),
                VrstaMaterijala.KamenaVuna => Math.Round(1.5 * Debljina, 2),
                _ => 0
            };

        
        
        public double CijenaLjepila => 5.2;
        public double CijenaZavrsnogSloja => 4.5;
        public double CijenaMrezice => 1;
        public double CijenaRada => 23;
        
        public double CijenaUkupnoPoM2 =>
            CijenaPoM2 + CijenaLjepila + CijenaZavrsnogSloja + CijenaMrezice + CijenaRada;
        
        public double UkupnaCijena
        {
            get
            {
                var osnovna = UkupnaPovrsina * CijenaUkupnoPoM2;
                var dodatni = DodatniTroskovi?.Sum(t => t.Cijena) ?? 0;
                return Math.Round(osnovna + dodatni, 2);
            }
        }
    }
}


