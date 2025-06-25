using System.ComponentModel.DataAnnotations;

namespace Paup.Enum
{
    public enum VrstaMaterijala
    {
        [Display(Name = "XPS")]
        EPS,

        [Display(Name = "Stiropor")]
        Stiropor,

        [Display(Name = "Staklena vuna")]
        StaklenaVuna,

        [Display(Name = "Kamena vuna")]
        KamenaVuna
    }
}