namespace ProfileService.Models.DTOs
{
    public class ViewProfileReturnDTO
    {
        public string ProfileImage { get; set; }
        public string Intro { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string MaritalStatus { get; set; }
        public string OnBehalf { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string EyeColor { get; set; }
        public string HairColor { get; set; }
        public string Complexion { get; set; }
        public string BloodGroup { get; set; }
        public string Disability { get; set; }
        public string NativeLanguage { get; set; }
        public string Drink { get; set; }
        public string Smoke { get; set; }
        public string LivingWith { get; set; }
        public string Religion { get; set; }
        public string Caste { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string FatherStatus { get; set; }
        public string MotherStatus { get; set; }
        public int Noofsiblings { get; set; }
        public List<Educations> Educations { get; set; } = new List<Educations>();
        public List<Careers> Careers { get; set; } = new List<Careers>();
    }
}
