using System;
using System.Collections.Generic;
using System.Text;
using Soul_Talk.Models;
using System.IO;

namespace Soul_Talk.Models.Repositories
{
    // Simpelt repository for institutioner
    public class InstitutionRepository
    {
        public List<Institution> Institutioner { get; } = new List<Institution>();

        public List<Institution> HentAlle()
        {
            return Institutioner;
        }

        public void Tilfoej(Institution institution)
        {
            Institutioner.Add(institution);
        }

        // Gemmer alle institutioner i en tekstfil.
        // Én linje pr. institution: Id;Navn;Type
        public void GemTilFil(string sti)
        {
            using (StreamWriter writer = new StreamWriter(sti, false))
            {
                foreach (Institution inst in Institutioner)
                {
                    string linje = inst.Id + ";" + inst.Navn + ";" + inst.Type;
                    writer.WriteLine(linje);
                }
            }
        }
    }
}
