using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Paup.Models;
using Paup.Enum;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Paup.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppKorisnik> _userManager;
        private readonly SignInManager<AppKorisnik> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<AppKorisnik> userManager,
            SignInManager<AppKorisnik> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        
        [HttpGet]
        public IActionResult Register() => View();
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterView model)
{
            try
            {
                if (model.OdabranaUloga == VrstaKorisnika.Korisnik && model.Zupanija == null)
                    ModelState.AddModelError("Zupanija", "Županija je obavezna za korisnika.");

                if (model.OdabranaUloga == VrstaKorisnika.PružateljUsluga &&
                    (model.Zupanije == null || !model.Zupanije.Any()))
                    ModelState.AddModelError("Zupanije", "Odaberite barem jednu županiju.");

                if (await _userManager.FindByNameAsync(model.UserName) is not null)
                    ModelState.AddModelError(nameof(model.UserName), "Ime se već koristi.");

                if (await _userManager.FindByEmailAsync(model.Email) is not null)
                    ModelState.AddModelError(nameof(model.Email), "Email se već koristi.");

                if (!ModelState.IsValid)
                    return View(model);

                var user = new AppKorisnik
                {
                    UserName    = model.UserName,
                    Email       = model.Email,
                    KontaktBroj = model.KontaktBroj,
                    Uloga       = model.OdabranaUloga.ToString(),
                    Zupanije    = model.OdabranaUloga == VrstaKorisnika.Korisnik
                                  ? model.Zupanija?.ToString()
                                  : string.Join(',', model.Zupanije.Select(z => z.ToString()))
                };

                var rezultat = await _userManager.CreateAsync(user, model.Password);

                if (rezultat.Succeeded)
                {
                    var uloga = model.OdabranaUloga.ToString();

                    if (!await _roleManager.RoleExistsAsync(uloga))
                        await _roleManager.CreateAsync(new IdentityRole(uloga));

                    await _userManager.AddToRoleAsync(user, uloga);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var linkPotvrde = Url.Action("EmailPotvrda", "Account", new { userId = user.Id, token }, Request.Scheme);
                    Console.WriteLine("Confirmation link: " + linkPotvrde);

                    return RedirectToAction("PotvrdaEmail");
                }

                foreach (var error in rezultat.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(model);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Register] Greška: {ex.Message}\n{ex.StackTrace}");

                ModelState.AddModelError(string.Empty,
                    "Dogodila se neočekivana pogreška prilikom registracije. Pokušajte ponovno ili kontaktirajte podršku.");

                return View(model);
            }
}



        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginView model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(nameof(model.Email), "Korisnik ne postoji.");
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError(nameof(model.Email), "Email nije potvrđen.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError(string.Empty, "Pogrešan email ili lozinka.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Detalji()
        {
            var user = await _userManager.GetUserAsync(User);
            return user == null ? RedirectToAction("Login") : View(user);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UrediZupanije()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var zupanije = user.Zupanije?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(z => System.Enum.Parse<Zupanija>(z))
                .ToList() ?? new List<Zupanija>();

            return View(new EditZupanija
            {
                UserName = user.UserName,
                Email = user.Email,
                OdabraneZupanije = zupanije,
                JednaZupanija = zupanije.FirstOrDefault()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UrediZupanije(EditZupanija model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (user.Uloga == "Korisnik")
            {
                user.Zupanije = model.JednaZupanija?.ToString();
            }
            else if (user.Uloga == "PružateljUsluga")
            {
                user.Zupanije = model.OdabraneZupanije != null
                    ? string.Join(",", model.OdabraneZupanije.Select(z => z.ToString()))
                    : null;
            }

            await _userManager.UpdateAsync(user);
            return RedirectToAction("Detalji");
        }

        [HttpGet]
        public IActionResult PotvrdaEmail() => View("PotvrdaEmail");

        [HttpGet]
        public async Task<IActionResult> EmailPotvrda(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded
                ? View("EmailPotvrden")
                : View("Error");
        }
    }
}

