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
                    decimal lst_bem = data.Einkommen - ((await rep.GetSVSatzAsync(data.Einkommen) / 100) * data.Einkommen);
                    decimal eff_tarif = await rep.GetEffTarifAsync(lst_bem);
                    data.Ergebnis = lst_bem - eff_tarif;
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
         //public string TableName { get; set; }
    }
}
