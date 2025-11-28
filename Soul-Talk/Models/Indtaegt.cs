using System;

namespace Soul_Talk.Models
{
    public class Indtaegt
    {
        public int Id { get; set; }

        public Kunde Kunde { get; set; }

        public DateTime Dato { get; set; }

        public decimal Timer { get; set; }

        // True = fysisk, false = online
        public bool ErFysisk { get; set; }

        // Kørte kilometer i forbindelse med denne indtægt. 0 hvis der ikke er kørt noget.
        public decimal Kilometer { get; set; }

        public decimal Timepris { get; set; }

        public decimal Beloeb { get; set; }

        public Indtaegt()
        {
            Kunde = new Kunde();
        }
    }
}
