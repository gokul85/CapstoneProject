using AuthService.Models;

namespace AuthService.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user, bool isPremium);
    }
}
