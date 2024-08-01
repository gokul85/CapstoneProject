namespace ProfileService.Models
{
    public class PartnerPreference
    {
        public int Id { get; set; }
        public int MinHeight { get; set; }
        public int MaxHeight { get; set; }
        public int MinWeight { get; set; }
        public int MaxWeight { get; set; }
        public string MaritalStatus { get; set; }
        public string MinimumQualification { get; set; }
        public string Religion { get; set; }
        public string Language { get; set; }
        public bool SmokeAcceptable { get; set; }
        public bool DrinkAcceptable { get; set; }
        public string State { get; set; }
        public string Complexion { get; set; }
    }
}
