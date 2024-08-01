namespace ProfileService.Models
{
    public class ProfileImages
    {
        public int Id { get; set; }
        public UserProfile UserProfile { get; set; }
        public int UserProfileId { get; set; }
        public string Image { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
