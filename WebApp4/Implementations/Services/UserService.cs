using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using WebApp4.Abstractions;
using WebApp4.Contexts;
using WebApp4.DTOs.UserDTOs;
using WebApp4.Entities.Identities;
using WebApp4.Models;

namespace WebApp4.Implementations.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, IMapper mapper)
        {

            _userManager = userManager;

            _mapper = mapper;

        }
        public async Task<GenericResponseModel<bool>> AssignRoleToUserAsync(string userId, string[] roles)
        {
            GenericResponseModel<bool> responseModel = new GenericResponseModel<bool>()
            {
                Data = false,
                StatusCode = 400,
            };
            if (userId == null || roles == null)
            {
                return responseModel;
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return responseModel;
            }
            var availableRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, availableRoles);
            if (!roles.ToList().Contains("User"))
                await _userManager.AddToRoleAsync(user, "User");
            var result = await _userManager.AddToRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                return responseModel;
            }
            responseModel.Data = true;
            responseModel.StatusCode = 200;
            return responseModel;
        }

        public async Task<GenericResponseModel<CreateUserResponseDTO>> CreateAsync(CreateUserDTO userDTO)
        {
            GenericResponseModel<CreateUserResponseDTO> responseModel = new GenericResponseModel<CreateUserResponseDTO>()
            {
                Data = null,
                StatusCode = 400,
            };
            if (userDTO == null)
            {
                return responseModel;
            }
            var id = Guid.NewGuid().ToString();
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = id,
                UserName = userDTO.Username,
                Email = userDTO.Email,
                Firstname = userDTO.Firstname,
                LastName = userDTO.LastName,
            }, userDTO.Password);
            responseModel.Data = new CreateUserResponseDTO { Succeeded = result.Succeeded };
            responseModel.StatusCode = result.Succeeded ? 200 : 400;
            if (!result.Succeeded)
            {
                responseModel.Data.Message = string.Join("\n", result.Errors.Select(error => $"{error.Code}-{error.Description}"));
            }
            AppUser user = await _userManager.FindByNameAsync(userDTO.Username);
            if (user == null)
                user = await _userManager.FindByEmailAsync(userDTO.Email);
            if (user == null)
                user = await _userManager.FindByIdAsync(id);
            if (user != null)
                await _userManager.AddToRoleAsync(user, "User");
            return responseModel;
        }

        public async Task<GenericResponseModel<bool>> DeleteUserAsync(string userIdOrName)
        {
            GenericResponseModel<bool> responseModel = new GenericResponseModel<bool>()
            {
                Data = false,
                StatusCode = 400,
            };
            if (userIdOrName == null)
            {
                return responseModel;
            }
            var user = await _userManager.FindByIdAsync(userIdOrName);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(userIdOrName);
            }
            if (user == null)
            {
                return responseModel;
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return responseModel;
            }
            responseModel.StatusCode = 200;
            responseModel.Data = true;
            return responseModel;
        }

        public async Task<GenericResponseModel<List<GetUserDTO>>> GetAllUseAsyncr()
        {
            GenericResponseModel<List<GetUserDTO>> responseModel = new GenericResponseModel<List<GetUserDTO>>()
            {
                Data = null,
                StatusCode = 400,
            };
            var users = await _userManager.Users.ToListAsync();
            if (users.Count == 0)
            {
                return responseModel;
            }
            var user = _mapper.Map<List<GetUserDTO>>(users);
            responseModel.Data = user;
            responseModel.StatusCode = 200;
            return responseModel;
        }

        public async Task<GenericResponseModel<GetUserDTO>> GetById(string id)
        {
            GenericResponseModel<GetUserDTO> responseModel = new GenericResponseModel<GetUserDTO>()
            {
                Data = null,
                StatusCode = 400,
            };
            if (id == null)
            {
                return responseModel;
            }
            var user = await _userManager.FindByIdAsync(id);
            //_userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return responseModel;
            }
            responseModel.Data = _mapper.Map<GetUserDTO>(user);
            responseModel.StatusCode = 200;
            return responseModel;
        }

        public async Task<GenericResponseModel<string[]>> GetRolesToUserAsync(string userIdOrName)
        {
            GenericResponseModel<string[]> responseModel = new GenericResponseModel<string[]>()
            {
                Data = null,
                StatusCode = 400,
            };
            if (userIdOrName == null)
            {
                return responseModel;
            }
            var user = await _userManager.FindByIdAsync(userIdOrName);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(userIdOrName);
            }
            if (user == null)
            {
                return responseModel;
            }
            var roles = await _userManager.GetRolesAsync(user);
            string[] rolesArray = roles.ToArray();
            responseModel.Data = rolesArray;
            responseModel.StatusCode = 200;
            return responseModel;
        }

        public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenData)
        {
            //accessTokenDate
            var userToUpdate = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (userToUpdate != null)
            {
                userToUpdate.RefreshToken = refreshToken;
                userToUpdate.RefreshTokenEndTime = accessTokenData.AddMinutes(10);
                await _userManager.UpdateAsync(userToUpdate);
            }
        }
        public async Task<GenericResponseModel<bool>> UpdateUserAsync(UpdateUserDTO userDTO)
        {
            GenericResponseModel<bool> responseModel = new GenericResponseModel<bool>()
            {
                Data = false,
                StatusCode = 400,
            };
            if (userDTO == null)
            {
                return responseModel;
            }
            var user = await _userManager.FindByIdAsync(userDTO.Id);
            if(user == null) 
            {
                user = await _userManager.FindByNameAsync(userDTO.Username);
            }
            if (user == null)
            {
                return responseModel;
            }
            user.Id = userDTO.Id;
            user.Firstname = userDTO.Firstname;
            user.LastName = userDTO.LastName;
            user.Email = userDTO.Email;
            user.UserName = userDTO.Username;
            user.Birthday = userDTO.Birthday;
            var result = _userManager.UpdateAsync(user);
            if (result.IsCompletedSuccessfully)
            {
                responseModel.StatusCode = 200;
                responseModel.Data = true;
            }
            return responseModel;
        }
    }
}
