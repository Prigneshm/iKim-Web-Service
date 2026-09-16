using IKimWebService.Infrastructure.IService;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Configuration;
using System.Text;
using System;

namespace IKimWebService.Service
{
    public class JWTokenService : IJWTokenService
    {
        public string GenerateJwtToken(Domain.User mUser)
        {
            var key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings.Get("SecretKey"));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, $"{mUser.FirstName} {mUser.LastName}"),
                    new Claim("emailAddress", mUser.EmailAddress),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = ConfigurationManager.AppSettings.Get("Issuer"),
                Audience = ConfigurationManager.AppSettings.Get("Audience"),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
