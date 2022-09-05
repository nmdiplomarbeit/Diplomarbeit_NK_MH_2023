using LohnverrechnerGastro.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LohnverrechnerGastro.Controllers
{
    public class UserController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User userdaten)
        {
            if (userdaten == null)
            {
                return RedirectToAction("Login");
            }
            if (ModelState.IsValid)
            {
                if(userdaten.Name == "admin" && userdaten.Password == "admin")
                {
                    userdaten.IsLogged = true;
                    Logged = true;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(userdaten);
        }

        public IActionResult Logout()
        {
            if (Logged)
            {
                Logged = false;
                return RedirectToAction("Login");
            }
            return View("Index", "Home");       // falls nicht angemeldet kommt man einfach auf die Seite des Rechners
        }

        public static bool Logged { get; set; }
        
    }
}
