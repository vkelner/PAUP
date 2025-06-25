using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Paup.Models;
using Paup.Ostalo;


namespace Paup.Controllers

{
    public class KalkulatorController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.IsPruzateljUsluga = User.IsInRole("PružateljUsluga");
            
            return View(new Kalkulator());
        }

        [HttpPost]
        public IActionResult Index(Kalkulator model)
        {
            ViewBag.IsPruzateljUsluga = User.IsInRole("PružateljUsluga");

            if (!ModelState.IsValid)
                return View(model);

            ViewBag.Calculated = true;

            return View(model);
        }

        [HttpPost]
        public IActionResult GenerirajPdf(Kalkulator model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", model);
            }

            var generator = new KalkulatorPDF();
            var pdfBytes = generator.Generate(model);

            return File(pdfBytes, "application/pdf", "kalkulacija.pdf");
        }


    }
}