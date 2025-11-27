using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_Talk.Models.Services
{
    public interface ITimeprisService
    {
        decimal HentTimepris(Kunde kunde, bool erFysisk);
    }
}