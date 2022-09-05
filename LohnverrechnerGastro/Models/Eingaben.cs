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

    }
}
