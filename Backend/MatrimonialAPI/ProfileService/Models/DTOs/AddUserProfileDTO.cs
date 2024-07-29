﻿namespace ProfileService.Models.DTOs
{
    public class AddUserProfileDTO
    {
        public int UserId { get; set; }
        public string ProfileImage { get; set; }
        public string Bio { get; set; }
        public string NativeLanguage { get; set; }
        public string MaritalStatus { get; set; }
        public string Religion { get; set; }
        public string Caste { get; set; }
        public string HighestQualification { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Pincode { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string EyeColor { get; set; }
        public string HairColor { get; set; }
        public string Complexion { get; set; }
        public string BloodGroup { get; set; }
        public string Disability { get; set; }
        public string DisabilityDetails { get; set; }
        public string Father { get; set; }
        public string Mother { get; set; }
        public int SiblingsCount { get; set; }
        public string Drink { get; set; }
        public string Smoke { get; set; }
        public string LivingWith { get; set; }
        public List<string> GalleryImages { get; set; }
        public List<EducationDTO> Education { get; set; }
        public List<CareerDTO> Career { get; set; }
    }

    public class EducationDTO
    {
        public string Degree { get; set; }
        public string Specialization { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string Status { get; set; }
    }

    public class CareerDTO
    {
        public string Designation { get; set; }
        public string Company { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}
