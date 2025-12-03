using System;
using System.Collections.Generic;
using System.Text;
using Soul_Talk.Models;

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
    }
}
