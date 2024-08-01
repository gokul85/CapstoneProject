using System.Text.Json.Serialization;

namespace ProfileService.Models
{
    public class Educations
    {
        public int Id { get; set; }
        [JsonIgnore]
        public UserProfile UserProfile { get; set; }
        public int UserProfileId { get; set; }
        public string Degree { get; set; }
        public string Specialization { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string Status { get; set; }
    }
}
