using System;
using System.Collections.Generic;
using System.Text;
using Soul_Talk.Models;

namespace Soul_Talk.Models.Repositories
{
    // Simpelt repository der holder styr på kunder i en liste
    public class KundeRepository
    {
        // Intern liste med alle kunder
        public List<Kunde> Kunder { get; } = new List<Kunde>();

        // Hent alle kunder
        public List<Kunde> HentAlle()
        {
            return Kunder;
        }

        // Tilføj en kunde
        public void Tilfoej(Kunde kunde)
        {
            Kunder.Add(kunde);
        }
    }
}
