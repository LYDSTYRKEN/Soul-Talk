using System;
using System.Collections.Generic;
using System.Text;
using Soul_Talk.Models;
using System.IO;

namespace Soul_Talk.Models.Repositories
{
    // Simpelt repository for indtægter
    public class IndtaegtRepository
    {
        public List<Indtaegt> Indtaegter { get; } = new List<Indtaegt>();

        public List<Indtaegt> HentAlle()
        {
            return Indtaegter;
        }

        public void Tilfoej(Indtaegt indtaegt)
        {
            Indtaegter.Add(indtaegt);
        }

        // Gemmer alle indtægter i en tekstfil.
        // Én linje pr. indtægt:
        // KundeId;Dato;Timer;ErFysisk;Kilometer;Timepris;Beloeb
        public void GemTilFil(string sti)
        {
            using (StreamWriter writer = new StreamWriter(sti, false))
            {
                foreach (Indtaegt ind in Indtaegter)
                {
                    int kundeId = 0;
                    if (ind.Kunde != null)
                    {
                        kundeId = ind.Kunde.Id;
                    }

                    string linje =
                        kundeId + ";" +
                        ind.Dato.ToString("yyyy-MM-dd") + ";" +
                        ind.Timer + ";" +
                        ind.ErFysisk + ";" +
                        ind.Kilometer + ";" +
                        ind.Timepris + ";" +
                        ind.Beloeb;

                    writer.WriteLine(linje);
                }
            }
        }
    }
}
