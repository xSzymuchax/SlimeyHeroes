using Microsoft.IdentityModel.Tokens;
using ServerKlocki.Database.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServerKlocki.Helpers
{
    public class JWTTokenGenerator
    {
        private readonly IConfiguration _config;

        public JWTTokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Generates tokens for users. Register's name and id of user.
        /// </summary>
        /// <param name="user">name of token's owner</param>
        /// <returns>Token string</returns>
        public string GenerateJWTToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("id", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
