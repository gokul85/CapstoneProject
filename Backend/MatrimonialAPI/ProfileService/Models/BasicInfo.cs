namespace ProfileService.Models
{
    public class BasicInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DOB { get; set; }
        public string? Intro { get; set; }
        public string? NativeLanguage { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Religion { get; set; }
        public string? Caste { get; set; }
        public string? HighestQualification { get; set; }
    }
}
