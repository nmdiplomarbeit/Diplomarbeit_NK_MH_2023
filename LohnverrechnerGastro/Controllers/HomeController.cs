using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LohnverrechnerGastro.Models;


namespace LohnverrechnerGastro.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Eingaben EingabeDaten)
        {
            if(EingabeDaten == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                // komplette Rechenlogik
            }
            return View(EingabeDaten);
        }
        public IActionResult Impressum()
        {
            return View();
        }
    }
}
