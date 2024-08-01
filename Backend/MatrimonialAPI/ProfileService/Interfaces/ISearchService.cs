using ProfileService.Models.DTOs;

namespace ProfileService.Interfaces
{
    public interface ISearchService
    {
        public Task<ResponseModel> SearchProfiles(SearchCriteriaDTO searchCriteria,int userid);
        public Task<ResponseModel> ViewProfile(int profileid);
    }
}
