using System;
using System.Collections.Generic;
using System.Text;
using Soul_Talk.Models;

namespace Soul_Talk.Models.Repositories
{
    // Simpelt repository for indtægter
    public class IndtaegtRepository
    {
        public List<Indtaegt> Indtaegter { get; } = new List<Indtaegt>();

        public List<Indtaegt> HentAlle()
        {
            return Indtaegter;
        }

        public void Tilfoej(Indtaegt indtaegt)
        {
            Indtaegter.Add(indtaegt);
        }
    }
}

