using Paup.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Paup.Models
{
    public class PruzateljUsluge
    {
        public int Id { get; set; }

        public string AppKorisnikId { get; set; }

        public string NazivFirme { get; set; }

        public string Telefon { get; set; }

        public string Adresa { get; set; }

        public string Opis { get; set; }

        public string ZupanijeCSV { get; set; } = "";

        public bool Dostupan { get; set; }
        
        [NotMapped]
        public List<DateTime> SlobodniDani { get; set; } = new();
    }
}