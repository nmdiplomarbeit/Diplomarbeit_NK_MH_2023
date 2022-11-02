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
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(User userdaten)
        {
            if(userdaten == null)
            {
                return RedirectToAction("Resgistration");
            }
            RegistrationValidation(userdaten);
            if (ModelState.IsValid)
            {
                try
                {
                    await rep.ConnectAsync();
                    if (await rep.InsertAsync(userdaten))
                    {
                        Console.WriteLine("Daten gespeichert!");
                    }
                    else
                    {
                        Console.WriteLine("Daten nicht gespeichert!");
                    }
                }
                catch (DbException e)
                {
                    return View("_Errormessage",
                                new Errormessage("Registrierung", "Datenbankfehler!",
                                            "Bitte versuchen Sie es später erneut!"));
                }
                finally
                {
                    await rep.DisconnectAsync();
                }
            }
            return View(userdaten);
        }

        public void RegistrationValidation(User user)
        {
            Boolean keinKleinbuchstabe = false;
            Boolean keinGroßbuchstabe = false;

            if (user == null)
            {
                return;
            }
            if ((user.Name == null) || (user.Name.Trim().Length < 4))
            {
                ModelState.AddModelError("Name", "Der Benutzername muss mindestens 4 Zeichen lang sein");
            }
            if ((user.Password == null) || (user.Password.Length < 8))
            {
                ModelState.AddModelError("Password", "Das Passwort muss mindestens 8 Zeichen lang sein");
            }
            if(user.Password != null)
            {
                keinKleinbuchstabe = user.Password.ToUpper().Equals(user.Password);
                keinGroßbuchstabe = user.Password.ToLower().Equals(user.Password);
            }
            if ((keinKleinbuchstabe) || (keinGroßbuchstabe))
            {
                ModelState.AddModelError("Password", "Das Passwort muss Groß- und Kleinbuchstaben enthalten");
            }
            if ((!user.Email.Contains("@")) || (user.Email == null))
            {
                ModelState.AddModelError("EMail", "Die EMail sollte in dem EMail-Format (bsp.: maxmustermann@abc.com)");
            }
            for (int i = 0; i < 10; i++)
            {
                if (user.Password.Contains(i.ToString()))
                {
                    return;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                if (!user.Password.Contains(i.ToString()))
                {
                    ModelState.AddModelError("Password", "Das Passwort muss mindestens eine Zahl enthalten");
                }
            }
        }

        public async Task<IActionResult> checkEmailAsync(string email)
        {
            try
            {
                await rep.ConnectAsync();
                if (await rep.AskEmailAsync(email))
                {
                    return new JsonResult(true);
                }
                else
                {
                    return new JsonResult(false);
                }
            }
            //catch (DbException e)
            //{
            //    return View("_Errormessage",
            //                    new Errormessage("Anmeldung", "Datenbankfehler!",
            //                                "Bitte versuchen Sie es später erneut!"));
            //}
            finally
            {
                await rep.DisconnectAsync();

            }
        }

        public async Task<IActionResult> checkNameAsync(string email)
        {
            try
            {
                await rep.ConnectAsync();
                if (await rep.AskNameAsync(email))
                {
                    return new JsonResult(true);
                }
                else
                {
                    return new JsonResult(false);
                }
            }
            //catch (DbException e)
            //{
            //    return View("_Errormessage",
            //                    new Errormessage("Anmeldung", "Datenbankfehler!",
            //                                "Bitte versuchen Sie es später erneut!"));
            //}
            finally
            {
                await rep.DisconnectAsync();

            }
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
                        RepositoryUsersDB.IsLogged = true;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Console.WriteLine("Falsche Anmeldedaten");
                    }
                }
                catch (DbException)
                {
                    return View("_Errormessage",
                                new Errormessage("Anmeldung", "Datenbankfehler!",
                                            "Bitte versuchen Sie es später erneut!"));
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
            if (RepositoryUsersDB.IsLogged)
            {
                RepositoryUsersDB.IsLogged = false;
                RepositoryUsersDB.IsAdmin = false;
                return RedirectToAction("Login");
            }
            return View("Index", "Home");       // falls nicht angemeldet kommt man einfach auf die Seite des Rechners
        }
        
    }
}
