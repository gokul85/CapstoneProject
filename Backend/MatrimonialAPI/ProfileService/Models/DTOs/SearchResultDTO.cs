namespace ProfileService.Models.DTOs
{
    public class SearchResultDTO
    {
        public List<SearchProfileDTO> SearchProfiles { get; set; }
        public PartnerPreference PartnerPreference { get; set; }
    }
}
