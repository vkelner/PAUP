using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Paup.Models;

namespace Paup.Data
{
    public class AppDbContext : IdentityDbContext<AppKorisnik>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        
        public DbSet<PruzateljUsluge> PruzateljiUsluga { get; set; }
        
        public DbSet<Upit> Upiti { get; set; }
        
        public DbSet<Oprema> Opreme { get; set; }


    }
    
}