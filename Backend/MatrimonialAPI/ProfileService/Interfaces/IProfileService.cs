using ProfileService.Models;
using ProfileService.Models.DTOs;

namespace ProfileService.Interfaces
{
    public interface IProfileService
    {
        public Task<ResponseModel> CreateUserProfile(RegisterUserProfileDTO profile);
        public Task<ResponseModel> AddUserProfile(AddUserProfileDTO addprofile);
    }
}
