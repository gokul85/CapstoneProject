using System.Text.Json.Serialization;

namespace ProfileService.Models
{
    public class Careers
    {
        public int Id { get; set; }
        [JsonIgnore]
        public UserProfile UserProfile { get; set; }
        public int UserProfileId { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}
