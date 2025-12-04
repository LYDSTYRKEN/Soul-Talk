using System;
using System.Collections.Generic;
using System.Text;
using Soul_Talk.Models;
using System.IO;

namespace Soul_Talk.Models.Repositories
{
    // Simpelt repository der holder styr på kunder i en liste
    public class KundeRepository
    {
        public List<Kunde> Kunder { get; } = new List<Kunde>();

        public List<Kunde> HentAlle()
        {
            return Kunder;
        }

        public void Tilfoej(Kunde kunde)
        {
            Kunder.Add(kunde);
        }

        // Gemmer alle kunder i en tekstfil.
        // Én linje pr. kunde: Id;Navn;InstitutionId (0 = privat kunde)
        public void GemTilFil(string sti)
        {
            using (StreamWriter writer = new StreamWriter(sti, false))
            {
                foreach (Kunde k in Kunder)
                {
                    int instId = 0;
                    if (k.Institution != null)
                    {
                        instId = k.Institution.Id;
                    }

                    string linje = k.Id + ";" + k.Navn + ";" + instId;
                    writer.WriteLine(linje);
                }
            }
        }
    }
}
