using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_Talk.Models
{
    public class Indtaegt
    {
        public int Id { get; set; }

        public Kunde Kunde { get; set; } = null!;

        public DateTime Dato { get; set; }

        public decimal Timer { get; set; }

        public bool ErFysisk { get; set; }          // True = fysisk, false = online.
 
        public decimal Kilometer { get; set; }      // Kørte kilometer i forbindelse med denne indtægt. 0 hvis der ikke er kørt noget.

        public decimal Timepris { get; set; }

        public decimal Beloeb { get; set; }         // Alternativt: public decimal Beloeb => Timer * Timepris;
        
    }
}
