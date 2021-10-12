using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ydb.Domain.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ydb.Domain.Service
{
    public class JwtService : IJwtService
    {
        public IConfiguration Configuration { get; }

        public JwtService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GenerateToken(string username, string password)
        {
            var claims = new[] {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddMinutes(Convert.ToInt32(Configuration.GetSection("JWT")["Expires"]))).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT")["IssuerSigningKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                issuer: Configuration.GetSection("JWT")["ValidIssuer"],
                audience: Configuration.GetSection("JWT")["ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(Configuration.GetSection("JWT")["Expires"])),
                signingCredentials: creds);
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}
