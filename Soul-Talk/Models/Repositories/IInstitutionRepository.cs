using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_Talk.Models.Repositories
{
    public interface IInstitutionRepository
    {
        IReadOnlyCollection<Institution> HentAlle();
        Institution? HentVedId(int id);
        Institution Gem(Institution institution);
    }
}
