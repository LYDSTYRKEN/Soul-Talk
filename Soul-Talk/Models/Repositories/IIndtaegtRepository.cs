using System.Collections.Generic;

namespace Soul_Talk.Models.Repositories
{
    public interface IIndtaegtRepository
    {
        List<Indtaegt> HentForKunde(int kundeId);
        Indtaegt Gem(Indtaegt indtaegt);
    }
}
