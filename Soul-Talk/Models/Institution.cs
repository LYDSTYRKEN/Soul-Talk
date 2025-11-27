using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_Talk.Models
{
    public class Institution
    {
        public int Id { get; set; }

        public string Navn { get; set; } = string.Empty;

        public InstitutionType Type { get; set; }
    }
}