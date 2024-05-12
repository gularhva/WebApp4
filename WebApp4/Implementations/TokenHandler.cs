using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApp4.Abstractions;
using WebApp4.DTOs;
using WebApp4.Entities.Identities;

namespace WebApp4.Implementations
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        public TokenHandler(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<TokenDTO> CreateAccessToken(AppUser user)
        {
            TokenDTO tokenDTO = new TokenDTO();

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            //Convert.ToInt32(_configuration["JWT:JWTExpireTime"])
            tokenDTO.Expiration = DateTime.UtcNow.AddMinutes(15);
            JwtSecurityToken securityToken = new(
                audience: _configuration["JWT:Audience"],
                issuer: _configuration["JWT:Issuer"],
                expires: tokenDTO.Expiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: claims
                );

            //Token yaradiriq
            JwtSecurityTokenHandler tokenHandler = new();
            tokenDTO.AccessToken = tokenHandler.WriteToken(securityToken);

            //refresh token yaradib veririk
            tokenDTO.RefreshToken = CreateRefreshToken();

            return tokenDTO;
        }

        public string CreateRefreshToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:RefreshTokenSecret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var refreshToken= tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(refreshToken);

        }
    }
}
