using System;

namespace Soul_Talk.Models.Services
{
    public class TimeprisService : ITimeprisService
    {
        public decimal HentTimepris(Kunde kunde, bool erFysisk)
        {
            if (kunde == null)
            {
                // Simpelt fallback. Burde egentlig aldrig ske.
                return 0;
            }

            // Privat kunde (ingen institution)
            if (kunde.Institution == null)
            {
                if (erFysisk)
                {
                    return 450m;
                }
                else
                {
                    return 350m;
                }
            }

            // Offentlig institution
            if (kunde.Institution.Type == InstitutionType.Offentlig)
            {
                if (erFysisk)
                {
                    return 550m;
                }
                else
                {
                    return 550m;
                }
            }

            // Privat institution
            if (erFysisk)
            {
                return 450m;
            }
            else
            {
                return 350m;
            }
        }
    }
}
