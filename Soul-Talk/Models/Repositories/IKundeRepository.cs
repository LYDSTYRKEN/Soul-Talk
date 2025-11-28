using System.Collections.Generic;

namespace Soul_Talk.Models.Repositories
{
    public interface IKundeRepository
    {
        List<Kunde> HentAlle();
        Kunde Gem(Kunde kunde);
    }
}
