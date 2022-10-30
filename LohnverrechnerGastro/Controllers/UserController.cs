using LohnverrechnerGastro.Models;
using LohnverrechnerGastro.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace LohnverrechnerGastro.Controllers
{
    public class UserController : Controller
    {

        private IRepositoryUsersDB rep = new RepositoryUsersDB();

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
        public async Task<IActionResult> LoginAsync(User userdaten)
        {
            if (userdaten == null)
            {
                return RedirectToAction("Login");
            }
            //if (ModelState.IsValid)
            //{
            //    if(userdaten.Name == "admin" && userdaten.Password == "admin")
            //    {
            //        userdaten.IsLogged = true;
            //        Logged = true;
            //        return RedirectToAction("Index", "Home");
            //    }
            //}
            //return View(userdaten);
            if (ModelState.IsValid)
            {
                try
                {
                    await rep.ConnectAsync();
                    if (await rep.LoginAsync(userdaten.Name, userdaten.Password))
                    {
                        userdaten.IsLogged = true;
                        Logged = true;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Console.WriteLine("Falsche Anmeldedaten");
                    }
                }
                catch (DbException)
                {
                    Console.WriteLine("Datenbankfehler");
                }
                finally
                {
                    await rep.DisconnectAsync();
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
