namespace Soul_Talk.Models
{
    // Repræsenterer en institution i systemet
    // Det kan f.eks. være en kommune eller en privat institution
    public class Institution
    {
        // Unikt id for institutionen
        public int Id { get; set; }

        // Navnet på institutionen, f.eks. "Odense Kommune" eller "Skovbrynet"
        public string Navn { get; set; }

        // Om institutionen er Offentlig eller Privat
        public InstitutionType Type { get; set; }

        // Constructor: sikrer at Navn starter som tom tekst og ikke er null
        public Institution()
        {
            Navn = "";
        }
    }
}
