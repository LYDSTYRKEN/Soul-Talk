namespace Soul_Talk.Models.Services
{
    // Service-klasse der beregner timeprisen ud fra kunde og om mødet er fysisk/online
    public class TimeprisService
    {
        // Returnerer timeprisen i kroner
        // Regler:
        //  Offentlig institution:  Fysisk 550 / Online 550
        //  Privat institution:     Fysisk 450 / Online 350
        //  Privat kunde:           Fysisk 450 / Online 350

        public decimal HentTimepris(Kunde kunde, bool erFysisk)
        {
            if (kunde == null)
            {
                // Burde egentlig aldrig ske, men vi beskytter os
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
