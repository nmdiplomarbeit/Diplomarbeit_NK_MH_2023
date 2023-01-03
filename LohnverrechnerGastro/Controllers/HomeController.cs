using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LohnverrechnerGastro.Models;
using LohnverrechnerGastro.Models.DB;
using System.Data.Common;
using OpenQA.Selenium.DevTools.V104.Database;

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
            if(data == null)
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
                    decimal lst_bem = data.Einkommen - ((await rep.GetSVSatzAsync(data.Einkommen) / 100) * data.Einkommen);
                    decimal eff_tarif = await rep.GetEffTarifAsync(lst_bem);
                    data.Ergebnis = lst_bem - eff_tarif;

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

        [Route("/Home/Tables/{TableName}")]
        public async Task<IActionResult> Tables(string TableName)
        {
            try
            {
                await rep.ConnectAsync();
                return View(await rep.GetAllTablesAsync(TableName));
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
        public async Task<IActionResult> DeleteAsync(int cnumber, string tablename)
        {
            try
            {
                await rep.ConnectAsync();
                await rep.DeleteAsync(cnumber, tablename);
                return RedirectToAction("Index");
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
    }
}
