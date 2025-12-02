using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_Talk.Models.Forretningslogik
{
    public interface ITimeBeregner
    {
        decimal HentTimepris(Kunde kunde, bool erFysisk);
    }
}
