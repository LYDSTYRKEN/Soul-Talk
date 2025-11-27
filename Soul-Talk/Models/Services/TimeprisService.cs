using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_Talk.Models.Services
{ 
     public class TimeprisService : ITimeprisService
    {
        public decimal HentTimepris(Kunde kunde, bool erFysisk)
        {
            if (kunde == null) throw new ArgumentNullException(nameof(kunde));

            // Privat kunde (ingen institution)
            if (kunde.Institution is null)
            {
                return erFysisk ? 450m : 350m;
            }

            // Institutionskunder
            if (kunde.Institution.Type == InstitutionType.Offentlig)
            {
                return erFysisk ? 550m : 450m;
            }

            // Privat institution
            return erFysisk ? 450m : 350m;
        }
    }
}

