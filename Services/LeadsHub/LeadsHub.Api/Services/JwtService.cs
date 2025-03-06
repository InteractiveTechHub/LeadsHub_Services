using LeadsHub.Core.Identity.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LeadsHub.Api.Services
{
    public class JwtService(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateToken(ApplicationUser user, IList<string> userRoles)
        {
            IConfiguration jwtSettings = _configuration.GetSection("JwtSettings");
            SymmetricSecurityKey secretKey = new(Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("SecretKey")!));
            SigningCredentials credentials = new(secretKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            ];

            foreach (string role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                claims.Add(new Claim("role", role)); // Helper for frontend
            }

            JwtSecurityToken token = new(
                issuer: jwtSettings.GetValue<string>("Issuer"),
                audience: jwtSettings.GetValue<string>("Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddHours(jwtSettings.GetValue<int>("ExpiresInHours")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
