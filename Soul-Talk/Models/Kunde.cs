namespace Soul_Talk.Models
{
    public class Kunde
    {
        public int Id { get; set; }

        public string Navn { get; set; }

        // Null = privat kunde. Ikke null = institutionsklient.
        public Institution Institution { get; set; }

        public bool ErPrivatKunde
        {
            get
            {
                return Institution == null;
            }
        }

        public Kunde()
        {
            Navn = "";
        }
    }
}
