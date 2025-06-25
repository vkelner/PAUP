using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Paup.Data;
using Paup.Models;
using Paup.Enum;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Paup.Controllers
{
    [Authorize]
    public class UslugeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppKorisnik> _userManager;

        public UslugeController(AppDbContext context, UserManager<AppKorisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            
            if (!User.Identity.IsAuthenticated)
            {
                return View("RedirectView");
            }


            var user = await _userManager.GetUserAsync(User);

            if (user.Uloga?.ToLower() == "korisnik")
            {
                var korisnikovaZupanija = user.Zupanije?.Trim();

                var sviPruzatelji = await _context.PruzateljiUsluga
                    .Where(p => p.Dostupan)
                    .ToListAsync();

                foreach (var p in sviPruzatelji)
                {
                    if (p.ZupanijeCSV != null)
                    {
                        var zupanije = p.ZupanijeCSV.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(z => z.Trim().ToLower());
                    }
                }

                var izvodaci = sviPruzatelji
                    .Where(p => p.ZupanijeCSV != null &&
                                p.ZupanijeCSV
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Any(z => string.Equals(z.Trim(), korisnikovaZupanija, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
                
                var danas = DateTime.Today;
                var slobodniDatumi = Enumerable.Range(0, 120)
                    .Select(i => danas.AddDays(i))
                    .Where(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                    .ToList();


                foreach (var pruzatelj in izvodaci)
                {
                    var upiti = _context.Upiti
                        .Where(u => u.PrimateljId == pruzatelj.AppKorisnikId && u.DatumRezervacije != null && u.Prihvacen)
                        .ToList();

                    var zauzeti = upiti
                        .SelectMany(u =>
                            Enumerable.Range(0, u.TrajanjeDana ?? 1)
                                .Select(i => (u.DatumRezervacije ?? DateTime.MinValue).AddDays(i)))
                        .ToList();

                    pruzatelj.SlobodniDani = slobodniDatumi
                        .Where(d => !zauzeti.Contains(d))
                        .ToList();
                }

                foreach (var p in sviPruzatelji)
                {
                    var zupanije = p.ZupanijeCSV?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(z => z.Trim().ToLower());
                }


                return View("Lista", izvodaci);
            }

            if (user.Uloga?.ToLower() == "pružateljusluga")
            {
                var pruzatelj = await _context.PruzateljiUsluga
                    .FirstOrDefaultAsync(p => p.AppKorisnikId == user.Id);

                if (pruzatelj == null)
                {
                    return RedirectToAction("Prijava");
                }

                var upiti = await _context.Upiti
                    .Where(u => u.PrimateljId == user.Id && !u.Odbijen && !u.Prihvacen)
                    .OrderByDescending(u => u.Vrijeme)
                    .ToListAsync();

                return View("Upiti", upiti);
            }


            return RedirectToAction("Index", "Home");
        }


        public IActionResult Prijava()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Prijava(PruzateljUsluge model, List<string> odabraneZupanije)
        {
            var user = await _userManager.GetUserAsync(User);
            var existing = await _context.PruzateljiUsluga
                .FirstOrDefaultAsync(p => p.AppKorisnikId == user.Id);

            var cleanZupanije = new List<string>();

            foreach (var z in odabraneZupanije)
            {
                if (System.Enum.TryParse<Paup.Enum.Zupanija>(z, out var enumVal))
                {
                    cleanZupanije.Add(enumVal.ToString());
                }
            }

            var zupanijeCsv = string.Join(",", cleanZupanije);


            if (existing != null)
            {
                existing.NazivFirme = model.NazivFirme;
                existing.Telefon = model.Telefon;
                existing.Adresa = model.Adresa;
                existing.Opis = model.Opis;
                existing.ZupanijeCSV = zupanijeCsv;
                existing.Dostupan = true;
            }
            else
            {
                model.AppKorisnikId = user.Id;
                model.ZupanijeCSV = zupanijeCsv;
                model.Dostupan = true;
                _context.PruzateljiUsluga.Add(model);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PosaljiUpit(Upit model)
        {
            if (!ModelState.IsValid)
            {
                TempData["UpitPoslan"] = null;
                var sviPruzatelji = await _context.PruzateljiUsluga.ToListAsync();
                return View("Lista", sviPruzatelji);
            }
            bool datumZauzet = await _context.Upiti.AnyAsync(u =>
                u.PrimateljId == model.PrimateljId &&
                u.DatumRezervacije == model.DatumRezervacije);

            if (datumZauzet)
            {
                ModelState.AddModelError("", "Odabrani datum je već zauzet.");
                var sviPruzatelji = await _context.PruzateljiUsluga.ToListAsync();
                return View("Lista", sviPruzatelji);
            }

            var posiljatelj = await _userManager.GetUserAsync(User);

            var upit = new Upit
            {
                PosiljateljId = posiljatelj.Id,
                PrimateljId = model.PrimateljId,
                ImePrezime = model.ImePrezime,
                Adresa = model.Adresa,
                Grad = model.Grad,
                Poruka = model.Poruka,
                Vrijeme = DateTime.Now,
                DatumRezervacije = model.DatumRezervacije,
                Telefon = model.Telefon
            };

            _context.Upiti.Add(upit);
            await _context.SaveChangesAsync();

            TempData["UpitPoslan"] = "Upit je uspješno poslan.";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UrediZupanije(EditZupanija model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Korisnik"))
            {
                user.Zupanije = model.JednaZupanija?.ToString();
            }
            else if (User.IsInRole("PružateljUsluga"))
            {
                var pruzatelj = await _context.PruzateljiUsluga
                    .FirstOrDefaultAsync(p => p.AppKorisnikId == user.Id);

                if (pruzatelj != null)
                {
                    var cleanZupanije = model.OdabraneZupanije
                        .Select(z => z.ToString())
                        .ToList();

                    pruzatelj.ZupanijeCSV = string.Join(",", cleanZupanije);
                    if (cleanZupanije.Any())
                    {
                        user.Zupanije = cleanZupanije.First();
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Usluge");
        }


        [Authorize(Roles = "Korisnik")]
        public async Task<IActionResult> MojiUpiti()
        {
            var user = await _userManager.GetUserAsync(User);
            var upiti = await _context.Upiti
                .Where(u => u.PosiljateljId == user.Id)
                .OrderByDescending(u => u.Vrijeme)
                .ToListAsync();

            return View(upiti);
        }

        [HttpPost]
        [Authorize(Roles = "Korisnik")]
        public async Task<IActionResult> ObrisiUpit(int id)
        {
            var upit = await _context.Upiti.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);

            if (upit == null || upit.PosiljateljId != user.Id)
                return Forbid();

            _context.Upiti.Remove(upit);
            await _context.SaveChangesAsync();

            return RedirectToAction("MojiUpiti");
        }
        
        [HttpPost]
        [Authorize(Roles = "PružateljUsluga")]
        public async Task<IActionResult> OdbijUpit(int id, string razlog, string dodatniRazlog)
        {
            var upit = await _context.Upiti.FindAsync(id);
            if (upit != null)
            {
                upit.Odbijen = true;
                upit.OdbijenRazlog = string.IsNullOrWhiteSpace(dodatniRazlog) ? razlog : dodatniRazlog;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        
        [HttpPost]
        [Authorize(Roles = "PružateljUsluga")]
        public async Task<IActionResult> PrihvatiUpit(int id, int trajanjeDana)
        {
            var upit = await _context.Upiti.FindAsync(id);
            if (upit == null)
                return NotFound();

            upit.Prihvacen = true;
            upit.TrajanjeDana = trajanjeDana;
            upit.OdbijenRazlog = "Prihvaćeno - pružatelj će vas kontaktirati.";

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "PružateljUsluga")]
        public async Task<IActionResult> PrihvaceniUpiti()
        {
            var user = await _userManager.GetUserAsync(User);

            var prihvaceni = await _context.Upiti
                .Where(u => u.PrimateljId == user.Id && u.Prihvacen)
                .OrderByDescending(u => u.Vrijeme)
                .ToListAsync();

            return View("PrihvaceniUpiti", prihvaceni);
        }

        [HttpPost]
        [Authorize(Roles = "PružateljUsluga")]
        public async Task<IActionResult> ObrisiPrihvaceniUpit(int id)
        {
            var upit = await _context.Upiti.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);

            if (upit == null || upit.PrimateljId != user.Id || !upit.Prihvacen)
                return Forbid();

            _context.Upiti.Remove(upit);
            await _context.SaveChangesAsync();

            return RedirectToAction("PrihvaceniUpiti");
        }

    }
}



