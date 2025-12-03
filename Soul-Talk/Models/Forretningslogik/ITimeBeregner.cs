using System;
using System.Collections.Generic;
using System.Text;
using Soul_Talk.Models;

namespace Soul_Talk.Models.Forretningslogik
{
    public interface ITimeBeregner
    {
        decimal HentTimepris(Kunde kunde, bool erFysisk);
    }
}
