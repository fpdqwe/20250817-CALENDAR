using BLL.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL.Utilities
{
    public class JwtTokenProvider : ITokenProvider
    {
        private readonly JwtOptions _options;
        public JwtTokenProvider(IOptions<JwtOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options.Value, nameof(options));
            ArgumentNullException.ThrowIfNullOrEmpty(options.Value.Key, nameof(options.Value.Key));
            _options = options.Value;
        }
        public string Generate(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_options.Lifetime),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
