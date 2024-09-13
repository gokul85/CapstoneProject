using AuthService.Interfaces;
using AuthService.Models;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            var kvUri = configuration.GetConnectionString("keyvaulturi");
            var clientId = configuration["Azure_Client_ID"];

            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                ManagedIdentityClientId = clientId
            }));

            var secret = client.GetSecretAsync("JWTTokenKey").GetAwaiter().GetResult();
            _secretKey = secret.Value.Value;

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        }
        public string GenerateToken(User user, bool isPremium)
        {
            string token = string.Empty;
            var claims = new List<Claim>(){
                new Claim("uid",user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim("scope",user.Role),
                new Claim("isPremium", isPremium.ToString()),
            };
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var myToken = new JwtSecurityToken(null, null, claims, expires: DateTime.Now.AddDays(2), signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(myToken);
            return token;
        }
    }
}
