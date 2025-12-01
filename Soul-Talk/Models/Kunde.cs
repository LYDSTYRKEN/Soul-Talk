namespace Soul_Talk.Models
{
    // Repræsenterer én kunde/klient i systemet
    public class Kunde
    {
        // Unikt id for kunden (bruges til at skelne kunderne i lister osv.)
        public int Id { get; set; }

        // Kundens navn (f.eks. "Peter Hansen")
        public string Navn { get; set; }

        // Hvis Institution er null -> privat kunde
        // Hvis Institution IKKE er null -> kunden hører til en institution (kommune, privat institution osv.)
        public Institution Institution { get; set; }

        // Hjælpe-property:
        // Returnerer true, hvis kunden er privat (dvs. ingen institution tilknyttet)
        public bool ErPrivatKunde
        {
            get
            {
                return Institution == null;
            }
        }

        // Constructor: sørger for at Navn aldrig er null (starter som tom tekst)
        public Kunde()
        {
            Navn = "";
        }
    }
}
