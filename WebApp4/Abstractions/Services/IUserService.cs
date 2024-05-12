using WebApp4.DTOs.UserDTOs;
using WebApp4.Entities.Identities;
using WebApp4.Models;

namespace WebApp4.Abstractions
{
    public interface IUserService
    {
        public Task<GenericResponseModel<List<GetUserDTO>>> GetAllUseAsyncr();
        public Task<GenericResponseModel<GetUserDTO>> GetById(string id);
        public Task<GenericResponseModel<CreateUserResponseDTO>> CreateAsync(CreateUserDTO userDTO);
        public Task<GenericResponseModel<bool>> UpdateUserAsync(UpdateUserDTO userDTO);
        public Task<GenericResponseModel<bool>> DeleteUserAsync(string userIdOrName);
        public Task<GenericResponseModel<string[]>> GetRolesToUserAsync(string userIdOrName);
        public Task<GenericResponseModel<bool>> AssignRoleToUserAsync(string userId, string[] roles);
        Task UpdateRefreshToken(string  refreshToken,AppUser user,DateTime accessTokenData);
    }
}
