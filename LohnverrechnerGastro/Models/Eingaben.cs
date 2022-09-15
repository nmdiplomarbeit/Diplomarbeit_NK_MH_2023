using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LohnverrechnerGastro.Models
{
    public class Eingaben
    {

        private double einkommen;

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

        public double Einkommen {
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
                    return Kategorie.keineKat;
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
    }
}
