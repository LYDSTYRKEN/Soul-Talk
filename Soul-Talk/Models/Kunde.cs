using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_Talk.Models
{
    public class Kunde
    {
        public int Id { get; set; }

        public string Navn { get; set; } = string.Empty;    

        public Institution? Institution { get; set; }       // Null = privat kunde. Ikke null = institutionsklient.

        public bool ErPrivatKunde => Institution is null;
    }
}