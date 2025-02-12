using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GamingCenter.Application.Interfaces;
using GamingCenter.Domain.Entities;
using GamingCenter.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GamingCenter.Infrastructure.Services
{
    /// <summary>
    /// Service responsible for generating JSON Web Tokens (JWT) for authentication.
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtService"/> class with the provided JWT settings.
        /// </summary>
        /// <param name="JwtSettings">The JWT settings configured in the application.</param>
        public JwtService(IOptions<JwtSettings> JwtSettings)
        {
            _jwtSettings = JwtSettings.Value;
        }

        /// <summary>
        /// Generates a JWT token for a specified user.
        /// </summary>
        /// <param name="user">The user for whom the JWT token will be generated.</param>
        /// <returns>A JWT token as a string used for user authentication.</returns>
        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email.Value),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
