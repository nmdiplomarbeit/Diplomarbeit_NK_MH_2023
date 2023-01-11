using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LohnverrechnerGastro.Models;
using LohnverrechnerGastro.Models.DB;
using System.Data.Common;
using OpenQA.Selenium.DevTools.V104.Database;
using Org.BouncyCastle.Asn1.X509;

namespace LohnverrechnerGastro.Controllers
{
    public class HomeController : Controller
    {
        private IRepositoryTablesDB rep = new RepositoryTablesDB();

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Eingaben data)
        {
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                // komplette Rechenlogik
                try
                {
                    await rep.ConnectAsync();
                    /*if (await rep.GetSVSatzAsync(data.Einkommen) != 0)
                    {

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Console.WriteLine("Daten nicht gespeichert!");
                    }*/

                    // einfaches Beispiel rechnen
                    decimal anzahl_682 = 0;
                    decimal lst_bem = data.Einkommen - ((await rep.GetSVSatzAsync(data.Einkommen) / 100) * data.Einkommen);
                    decimal eff_tarif = await rep.GetEffTarifAsync(lst_bem);
                    if (data.StundenproWoche > 40 && data.StundenproWoche < 42.5)
                    {
                        anzahl_682 = (decimal)Math.Round(((data.StundenproWoche - 40) * 4.33));
                    }
                    data.Ergebnis = lst_bem - eff_tarif;
                    //data.Ergebnis = lst_bem - eff_tarif + (anzahl_682*(await rep.GetSVSatzAsync(data.Einkommen) / 2));
                    //data.Ergebnis = (anzahl_682 * (await rep.GetSVSatzAsync(data.Einkommen) / 2));

                    // DG Abgaben Dictionary testen
                    //Dictionary<string, decimal> dic = await rep.GetDGAbgaben();
                    //data.Ergebnis = dic["sv_dg"];

                    // Grenzen SV Dictionary testen
                    //Dictionary<string, decimal> dic = await rep.GetGrenzenSV();
                    //Dictionary<string, decimal> dic2 = await rep.GetGrenzenSVSZ();
                    //data.Ergebnis = dic["hbgl"] + dic2["hbgl_sz"];

                    // SZ Steuergrenzen testen
                    //data.Ergebnis = await rep.GetSZSteuergrenzen(data.Einkommen);



                }
                catch (DbException e)
                {
                    return View("Index");
                }
                finally
                {
                    await rep.DisconnectAsync();
                }
            }
            return View(data);
        }

        [Route("/Home/Tables/{tablename}")]
        public async Task<IActionResult> Tables(string tablename)
        {
            try
            {
                await rep.ConnectAsync();
                return View(await rep.GetAllTablesAsync(tablename));
            }
            catch (DbException)
            {
                return View("Index");
            }
            finally
            {
                await rep.DisconnectAsync();
            }
        }

        [HttpGet]
        [Route("/Home/DeleteAsync/{tablename}/{cnumber}")]
        public async Task<IActionResult> DeleteAsync(string tablename, int cnumber)
        {
            try
            {
                await rep.ConnectAsync();
                await rep.DeleteAsync(tablename, cnumber);
                return RedirectToAction("Index");
            }
            catch (DbException e)
            {
                return View("Index");
            }
            finally
            {
                await rep.DisconnectAsync();
            }
        }

        [HttpGet]
        [Route("/Home/UpdateAsync/{tablename}/{cnumber}")]
        public async Task<IActionResult> UpdateAsync(string tablename, int cnumber)
        {
            try
            {
                await rep.ConnectAsync();
                // Liste mit allen Usern an die View übergeben
                return View(await rep.GetOneTableRow(tablename, cnumber));
            }
            catch (DbException)
            {
                return View("Index");
            }
            finally
            {
                await rep.DisconnectAsync();
            }
            return View();
        }

        [HttpPost]
        [Route("/Home/UpdateAsync/{tablename}/{cnumber}")]
        public async Task<IActionResult> UpdateAsync(Table t, int cnumber, string tablename)
        {

            if (t == null)
            {
                return RedirectToAction("update");
            }

            try
            {
                await rep.ConnectAsync();
                if (await rep.UpdateAsync(tablename, cnumber, t))
                {
                    return View("Index");
                }
                else
                {
                    return View("Tables");
                }
            }
            catch (DbException)
            {
                return View("Login");

            }
            finally
            {
                await rep.DisconnectAsync();
            }
            //falls etwas falsch eingeg. wurde, wird das Reg-formular
            // erneut aufgerufen - mit dne bereits eingegebnenen Daten.

            //return View(t);
        }

        [HttpGet]
        [Route("/Home/InsertAsync/{tablename}")]
        public IActionResult InsertAsync()
        {
            return View();
        }

        [HttpPost]
        [Route("/Home/InsertAsync/{tablename}")]
        public async Task<IActionResult> InsertAsync(string tablename, Table t)
        {

            if (t == null)
            {
                return RedirectToAction("update");
            }

            try
            {
                await rep.ConnectAsync();
                if (await rep.InsertAsync(tablename, t))
                {
                    return View("Index");
                }
                else
                {
                    return View("Tables");
                }
            }
            catch (DbException)
            {
                return View("Login");

            }
            finally
            {
                await rep.DisconnectAsync();
            }
            //falls etwas falsch eingeg. wurde, wird das Reg-formular
            // erneut aufgerufen - mit dne bereits eingegebnenen Daten.

            //return View(t);
        }

    }


}
