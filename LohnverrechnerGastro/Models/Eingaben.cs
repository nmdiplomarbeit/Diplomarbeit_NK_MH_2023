using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LohnverrechnerGastro.Models
{
    public class Eingaben
    {

        private decimal einkommen;

        private double stundenprowoche;

        private double tgpauschale;

        private double sachbezug;

        private double fkersatz;

        private double dnbeitrag;

        private double lstfreibetrag;

        private int anzkinderbis17;

        private int anzkinderab18;

        private int anzKinder;

        private int anzkilometer;

        private bool halberBonus;

        private Kategorie kategorie;


        public Bundesland Bundesland { get; set; }

        public int Jahr { get; set; }

        public string Brutodnet { get; set; } // Brutto 0, Netto 1

        public decimal Einkommen {
            get { return this.einkommen; }
            set
            {
                if (value > 0)
                {
                    this.einkommen = value;
                }
            }
        }

        public double StundenproWoche
        {
            get { return this.stundenprowoche; }
            set
            {
                if (value >= 0)
                {
                    this.stundenprowoche = value;
                }
            }
        }

        public string SZvombrutodKV { get; set; }

        // Zusatzfunktionen
        
        public double TGPauschale {
            get { return this.tgpauschale; }
            set
            {
                if (value >= 0)
                {
                    this.tgpauschale = value;
                }
            }
        }

        public double Sachbezug
        {
            get { return this.sachbezug; }
            set
            {
                if (value >= 0)
                {
                    this.sachbezug = value;
                }
            }
        }

        public double FKErsatz
        {
            get { return this.fkersatz; }
            set
            {
                if (value >= 0)
                {
                    this.fkersatz = value;
                }
            }
        }

        public double DNBeitrag
        {
            get { return this.dnbeitrag; }
            set
            {
                if (value >= 0)
                {
                    this.dnbeitrag = value;
                }
            }
        }

        public double LstFreibetrag
        {
            get { return this.lstfreibetrag; }
            set
            {
                if (value >= 0)
                {
                    this.lstfreibetrag = value;
                }
            }
        }

        public bool FaBoPlus { get; set; }

        public bool AVABoAEAB { get; set; }

        public bool PendlerPauschale { get; set; }

        public bool HalberBonus {
            get { if (FaBoPlus == true) { 
                    return this.halberBonus; 
                  } else
                {
                    return false;
                }
            }
            set
            {
                if(FaBoPlus == true) {
                    this.halberBonus = value;
                }
            }
        }

        public int AnzKinderbis17 {
            get
            {
                if (FaBoPlus == true)
                {
                    return this.anzkinderbis17;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if ((value >= 0 && value <= 5) && FaBoPlus == true)
                {
                    this.anzkinderbis17 = value;
                }
            }    
        }

        public int AnzKinderab18
        {
            get
            {
                if (FaBoPlus == true)
                {
                    return this.anzkinderab18;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if ((value >= 0 && value <= 5) && FaBoPlus == true)
                {
                    this.anzkinderab18 = value;
                }
            }
        }

        public int AnzKinder
        {
            get
            {
                if (AVABoAEAB == true)
                {
                    return this.anzKinder;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if ((value >= 0 && value <= 5) && AVABoAEAB == true)
                {
                    this.anzKinder = value;
                }
            }
        }

        public Kategorie Kategorie
        {
            get
            {
                if (PendlerPauschale == true)
                {
                    return this.kategorie;
                }
                else
                {
                    return Kategorie.notSpecified;
                }
            }
            set
            {
                if (PendlerPauschale == true)
                {
                    this.kategorie = value;
                }
            }
        }


        public int AnzKilometer
        {
            get
            {
                if (PendlerPauschale == true)
                {
                    return this.anzkilometer;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if ((value >= 0) && PendlerPauschale == true)
                {
                    this.anzkilometer = value;
                }
            }
        }

        public Eingaben(Bundesland bundesland, int jahr, string brutodnet, decimal einkommen,
            double stundenprowoche, string szvombruttoodKV) : this(bundesland, jahr, brutodnet, einkommen,
                stundenprowoche, szvombruttoodKV, 0, 0, 0, 0, 0, false, false, false, false, 0, 0, 0,
                Kategorie.notSpecified, 0){ }

        public Eingaben(Bundesland bundesland, int jahr, string brutodnet, decimal einkommen, 
            double stundenprowoche, string szvombruttoodKV, double tgpauschale, double sachbezug, 
            double fkersatz, double dnbeitrag, double lstfreibetrag,bool faboplus, bool avaboaeab, bool pendlerpauschale, 
            bool halberbonus, int anzkinderbis17, int anzkinderab18, int anzkinder, Kategorie kategorie, 
            int anzkilometer)
        {
            this.Bundesland = bundesland;
            this.Jahr = jahr;
            this.Brutodnet = brutodnet;
            this.Einkommen = einkommen;
            this.StundenproWoche = stundenprowoche;
            this.SZvombrutodKV = szvombruttoodKV;
            this.TGPauschale = tgpauschale;
            this.Sachbezug = sachbezug;
            this.FKErsatz = fkersatz;
            this.DNBeitrag = dnbeitrag;
            this.LstFreibetrag = lstfreibetrag;
            this.FaBoPlus = faboplus;
            this.AVABoAEAB = avaboaeab;
            this.PendlerPauschale = pendlerpauschale;
            this.HalberBonus = halberbonus;
            this.AnzKinderbis17 = anzkinderbis17;
            this.AnzKinderab18 = anzkinderab18;
            this.AnzKinder = anzkinder;
            this.Kategorie = kategorie;
            this.AnzKilometer = anzkilometer;
        }
    }
}
