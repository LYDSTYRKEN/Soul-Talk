using System.Collections.Generic;

namespace Soul_Talk.Models.Repositories
{
    public interface IInstitutionRepository
    {
        List<Institution> HentAlle();
        Institution Gem(Institution institution);
    }
}
