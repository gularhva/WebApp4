using WebApp4.DTOs;
using WebApp4.Entities.Identities;

namespace WebApp4.Abstractions
{
    public interface ITokenHandler
    {
        Task<TokenDTO> CreateAccessToken(AppUser user);
        string CreateRefreshToken();
    }
}
