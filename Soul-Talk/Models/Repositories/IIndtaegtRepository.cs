using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_Talk.Models.Repositories
{
    public interface IIndtaegtRepository
    {
        IReadOnlyCollection<Indtaegt> HentForKunde(
            int kundeId,
            DateTime? fra = null,
            DateTime? til = null);

        Indtaegt Gem(Indtaegt indtaegt);
    }
}