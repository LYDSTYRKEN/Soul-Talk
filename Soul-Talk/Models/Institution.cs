namespace Soul_Talk.Models
{
    public class Institution
    {
        public int Id { get; set; }

        public string Navn { get; set; }

        public InstitutionType Type { get; set; }

        public Institution()
        {
            Navn = "";
        }
    }
}
