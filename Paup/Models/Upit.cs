using System.ComponentModel.DataAnnotations;

namespace Paup.Models
{
    public class Upit
    {
        public int Id { get; set; }

        public string PosiljateljId { get; set; } = string.Empty;
        [Required]
        public string PrimateljId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ime i prezime je obavezno.")]
        [MaxLength(40, ErrorMessage = "Ime i prezime može imati najviše 40 znakova.")]
        public string ImePrezime { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Adresa stanovanja je obavezna.")]
        [MaxLength(40, ErrorMessage = "Adresa može imati najviše 40 znakova.")]
        public string Adresa { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Grad je obavezan.")]
        [MaxLength(30, ErrorMessage = "Ovo polje moze sadrzavati najvise 30 znakova.")]
        public string Grad { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Opis radova je obavezan.")]
        [MaxLength(300, ErrorMessage = "Ovo polje može sadržavati najviše 300 znakova.")]
        public string Poruka { get; set; } = string.Empty;
        public DateTime Vrijeme { get; set; } = DateTime.Now;
        
        public string? OdbijenRazlog { get; set; }
        public bool Odbijen { get; set; }
        
        [Required(ErrorMessage = "Datum je obavezan.")]
        public DateTime? DatumRezervacije { get; set; }
        public bool Prihvacen { get; set; }
        public int? TrajanjeDana { get; set; }
        
        [Required(ErrorMessage = "Broj telefona je obavezan.")]
        [MaxLength(20)]
        public string Telefon { get; set; } = string.Empty;

        
    }
}