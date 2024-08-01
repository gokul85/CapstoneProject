using PremiumService.Models.DTOs;

namespace PremiumService.Interfaces
{
    public interface IPremiumUserService
    {
        public Task<ResponseModel> SubscribePremium(SubscribePremiumDTO subscribePremiumDTO);
        public Task<ResponseModel> ContactView(int userid, int profileid);
        public Task<ResponseModel> CheckContactView(int userid, int profileid);
    }
}
