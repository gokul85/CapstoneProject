namespace ProfileService.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ProfileFor { get; set; }
        public BasicInfo BasicInfo { get; set; }
        public int? BasicInfoId { get; set; }
        public Address Address { get; set; }
        public int? AddressId { get; set; }
        public PhysicalAttributes PhysicalAttribute { get; set; }
        public int? PhysicalAttrId { get; set; }
        public FamilyInfo FamilyInfo { get; set; }
        public int? FamilyInfoId { get; set; }
        public PartnerPreference PartnerPref { get; set; }
        public int? PartnerPreId { get; set; }
        public Lifestyle LifeStyle { get; set; }
        public int? LifeStyleId { get; set; }
        public string? ProfileImage { get; set; }
        public bool ProfileCompleted { get; set; }
        public ICollection<Educations> Educations { get; set; }
        public ICollection<Careers> Careers { get; set; }
        public ICollection<ProfileImages> GallaryImages { get; set; }
    }
}
