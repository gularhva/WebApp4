using WebApp4.DTOs;
using WebApp4.Models;

namespace WebApp4.Abstractions.Services
{
    public interface IAuthService
    {
        public Task<GenericResponseModel<TokenDTO>> LoginAsync(string userNameOrEmail, string password);
        public Task<GenericResponseModel<TokenDTO>> LoginWithRefreshTokenAsync(string refreshToken);
        public Task<GenericResponseModel<bool>> LogOut(string userNameOrEmail);
        public Task<GenericResponseModel<bool>> ChangePasswordAsync(string email,string oldPassword,string newPassword);
    }
}
