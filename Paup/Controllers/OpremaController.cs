using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Paup.Data;
using Paup.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class OpremaController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppKorisnik> _userManager;

    public OpremaController(AppDbContext context, UserManager<AppKorisnik> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    
    [Authorize(Roles = "PružateljUsluga")]
    public IActionResult Dodaj()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "PružateljUsluga")]
    public async Task<IActionResult> Dodaj(Oprema model)
    {
        var user = await _userManager.GetUserAsync(User);
        model.PruzateljUslugeId = user.Id;
        
        if (ModelState.IsValid)
        {
            _context.Opreme.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("MojeOpreme");
        }

        return View(model);
    }

    [Authorize(Roles = "PružateljUsluga")]
    public async Task<IActionResult> MojeOpreme()
    {
        if (!User.Identity.IsAuthenticated)
            return View("RedirectView");
        
        var user = await _userManager.GetUserAsync(User);
        var mojaOprema = _context.Opreme.Where(o => o.PruzateljUslugeId == user.Id).ToList();
        return View(mojaOprema);
    }


    // GET: Oprema/Popis
    [AllowAnonymous]
    public IActionResult Popis()
    {
        if (!User.Identity.IsAuthenticated)
            return View("RedirectView");
        
        var dostupne = _context.Opreme
            .Where(o => o.Kolicina > 0)
            .Include(o => o.PruzateljUsluge)
            .ToList();

        return View(dostupne);

    }

    [HttpPost]
    [Authorize(Roles = "PružateljUsluga")]
    public async Task<IActionResult> Delete(int id)
    {
        var oprema = await _context.Opreme.FindAsync(id);
        if (oprema != null)
        {
            _context.Opreme.Remove(oprema);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("MojeOpreme");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "PružateljUsluga")]
    public async Task<IActionResult> UmanjiKolicinu(int id, int broj)
    {

        if (broj < 1) return RedirectToAction(nameof(MojeOpreme));

        var oprema = await _context.Opreme.FindAsync(id);
        if (oprema is null) return RedirectToAction(nameof(MojeOpreme));


        if (broj >= oprema.Kolicina)
        {
            _context.Opreme.Remove(oprema);
        }
        else
        {
            oprema.Kolicina -= broj;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(MojeOpreme));
    } 
    
    
}
    
    