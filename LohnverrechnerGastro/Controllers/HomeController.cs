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

                    decimal anzahl_682 = 0;
                    decimal anzahl_681 = 0;
                    decimal pendlerpau = 0;
                    decimal pendlereuro = data.AnzKilometer;
                    decimal kinder = data.AnzKinder;
                    decimal fabo = 0;
                    decimal grundwert = 0;
                    decimal kvsatz = 0;
                    Dictionary<string, decimal> hbgldic = await rep.GetGrenzenSV();
                    decimal hbgl = hbgldic["hbgl"];
                    decimal EinkommenSV = data.Einkommen;
                    decimal sv = 0;

                    if ((decimal)data.Kategorie == 0)
                    {
                        pendlerpau = await rep.GetPendler("20-40 km (klein)");
                    }
                    else if ((decimal)data.Kategorie == 1)
                    {
                        pendlerpau = await rep.GetPendler("40-60 km (klein)");

                    }
                    else if ((decimal)data.Kategorie == 2)
                    {
                        pendlerpau = await rep.GetPendler("ab 60 km (klein)");

                    }
                    else if ((decimal)data.Kategorie == 3)
                    {
                        pendlerpau = await rep.GetPendler("2-20 km (groß)");

                    }
                    else if ((decimal)data.Kategorie == 4)
                    {
                        pendlerpau = await rep.GetPendler("20-40 km (groß)");

                    }
                    else if ((decimal)data.Kategorie == 5)
                    {
                        pendlerpau = await rep.GetPendler("40-60 km (groß)");

                    }
                    else if ((decimal)data.Kategorie == 6)
                    {
                        pendlerpau = await rep.GetPendler("ab 60 km (groß)");

                    }
                    else
                    {
                        pendlerpau = 0;

                    }

                    if (EinkommenSV >= hbgl)
                    {
                        EinkommenSV = hbgl;
                        sv = ((await rep.GetSVSatzAsync((data.Einkommen)) / 100) * (EinkommenSV));
                    } else
                    {
                        sv = ((await rep.GetSVSatzAsync((data.Einkommen + (decimal)data.TGPauschale + (decimal)data.Sachbezug)) / 100) * (data.Einkommen + (decimal)data.TGPauschale + (decimal)data.Sachbezug));
                    }

                    decimal lst_bem1 = 0;
                    decimal lst_bem = ((data.Einkommen + (decimal)data.TGPauschale + (decimal)data.Sachbezug) - sv) - (decimal)data.TGPauschale + (decimal)data.FKErsatz - (decimal)data.LstFreibetrag - pendlerpau;

                    if (data.Angestellter)
                    {
                        kvsatz = (await rep.GetBeschaeftigungsgruppen(data.BeschaeftigungsGruppen) * await rep.GetBetrzugehAngestellter((int)data.Betriebszugehoerigkeit)) / 173;
                    }
                    else if (data.Arbeiter)
                    {
                        kvsatz = (await rep.GetLohngruppen(data.LohnGruppen) * await rep.GetBetrzugehArbeiter((int)data.Betriebszugehoerigkeit)) / 173;
                    }

                    if (data.StundenproWoche > 40 && data.StundenproWoche < 42.5)
                    {
                        anzahl_682 = (decimal)Math.Round((data.StundenproWoche - 40) * 4.33);
                        lst_bem1 = lst_bem - (anzahl_682 * ((decimal)kvsatz / 2));
                        anzahl_681 = 0;
                    }
                    else
                    {
                        lst_bem1 = lst_bem;
                        anzahl_681 = 0;
                    }

                    if (data.StundenproWoche >= 42.5 && data.StundenproWoche <= 48)
                    {
                        anzahl_682 = 10;
                        //anzahl_681 = ((decimal)Math.Round((data.StundenproWoche - 40) * 4.33) - 10);
                        anzahl_681 = data.Anzahl_681;
                        lst_bem1 = lst_bem1 - (anzahl_681 * ((decimal)kvsatz / 2)) - (anzahl_682 * ((decimal)kvsatz / 2));
                    }

                    decimal eff_tarif = await rep.GetEffTarifAsync(lst_bem1, (int)kinder);
                    if ((eff_tarif - (pendlereuro * (await rep.GetPendlerEuro()))) >= 0)
                    {
                        eff_tarif = eff_tarif - (pendlereuro * (await rep.GetPendlerEuro()));
                    }
                    else
                    {
                        eff_tarif = 0;
                    }
                    if (data.HalberBonus)
                    {
                        fabo = data.AnzKinderab18 * (await rep.GetFabo("ab18halb")) + data.AnzKinderbis17 * (await rep.GetFabo("bis17halb"));
                    }
                    else
                    {
                        fabo = data.AnzKinderab18 * (await rep.GetFabo("ab18")) + data.AnzKinderbis17 * (await rep.GetFabo("bis17"));
                    }

                    if ((eff_tarif - fabo) >= 0)
                    {
                        eff_tarif = eff_tarif - fabo;
                    }
                    else
                    {
                        eff_tarif = 0;
                    }

                    if (data.Angestellter)
                    {
                        if (((((decimal)Math.Round((data.StundenproWoche - 40) * 4.33) - 10))) <= 0)
                        {
                            anzahl_681 = 0;
                        }
                        else
                        {
                            anzahl_681 = (((decimal)Math.Round((data.StundenproWoche - 40) * 4.33) - 10));
                        }
                        grundwert = (await rep.GetBeschaeftigungsgruppen(data.BeschaeftigungsGruppen) * await rep.GetBetrzugehAngestellter((int)data.Betriebszugehoerigkeit)) + ((kvsatz * ((decimal)Math.Round(((decimal)data.StundenproWoche - (decimal)40) * (decimal)4.33))) + (anzahl_681 * (kvsatz / 2)) + (anzahl_682 * (kvsatz / 2)));
                    }
                    else if (data.Arbeiter)
                    {
                        if (((((decimal)Math.Round((data.StundenproWoche - 40) * 4.33) - 10))) <= 0)
                        {
                            anzahl_681 = 0;
                        }
                        else
                        {
                            anzahl_681 = (((decimal)Math.Round((data.StundenproWoche - 40) * 4.33) - 10));
                        }
                        grundwert = (await rep.GetLohngruppen(data.LohnGruppen) * await rep.GetBetrzugehArbeiter((int)data.Betriebszugehoerigkeit)) + ((kvsatz * ((decimal)Math.Round(((decimal)data.StundenproWoche - (decimal)40) * (decimal)4.33))) + (anzahl_681 * (kvsatz / 2)) + (anzahl_682 * (kvsatz / 2)));
                    }

                    if ((data.Einkommen >= grundwert) && ((data.Arbeiter) || (data.Angestellter)))
                    {
                        data.Netto = lst_bem - eff_tarif - (decimal)data.Sachbezug + (decimal)data.LstFreibetrag - (decimal)data.FKErsatz + pendlerpau;
                        data.Ergebnis = lst_bem - eff_tarif - (decimal)data.Sachbezug - (decimal)data.DNBeitrag + (decimal)data.LstFreibetrag + pendlerpau;
                    }
                    else
                    {
                        data.Grundwert = "Fehler! Das eingegebene Einkommen liegt unter dem mindest Grundwert! Grundwert: " + Math.Round((grundwert), 2);
                    }
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
            catch (DbException e)
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
        public IActionResult InsertAsync(/*string tablename*/)
        {
            //try
            //{
            //    await rep.ConnectAsync();
            //    // Liste mit allen Usern an die View übergeben
            //    return View(await rep.GetOneEmptyTableRow(tablename));
            //}
            //catch (DbException)
            //{
            //    return View("Index");
            //}
            //finally
            //{
            //    await rep.DisconnectAsync();
            //}
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
