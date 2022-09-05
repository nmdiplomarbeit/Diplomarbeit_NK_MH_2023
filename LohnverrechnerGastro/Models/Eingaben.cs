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
        

        
    }
}
