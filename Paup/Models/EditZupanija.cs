using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Paup.Enum;


public class EditZupanija
{
    public string? UserName { get; set; }
    public string? Email { get; set; }

    
    public Zupanija? JednaZupanija { get; set; }

    
    public List<Zupanija> OdabraneZupanije { get; set; } = new();

    public List<Zupanija> SveZupanije => Enum.GetValues(typeof(Zupanija)).Cast<Zupanija>().ToList();
}