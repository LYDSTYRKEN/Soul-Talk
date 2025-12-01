using System;

namespace Soul_Talk.Models
{
    // Repræsenterer én indtægt (arbejde udført for en kunde)
    public class Indtaegt
    {
        // Den kunde, som indtægten hører til
        public Kunde Kunde { get; set; }

        // Dato for arbejdet / indtægten
        public DateTime Dato { get; set; }

        // Antal timer, der er arbejdet
        public decimal Timer { get; set; }

        // True = fysisk møde hos kunden, false = online møde
        public bool ErFysisk { get; set; }

        // Kørte kilometer i forbindelse med denne opgave (0 hvis der ikke er kørt noget)
        public decimal Kilometer { get; set; }

        // Timepris i kroner, beregnet ud fra kunde + fysisk/online
        public decimal Timepris { get; set; }

        // Samlet beløb for denne indtægt (typisk Timer * Timepris)
        public decimal Beloeb { get; set; }

        // Constructor: sørger for at Kunde ikke er null
        public Indtaegt()
        {
            Kunde = new Kunde();
        }
    }
}
