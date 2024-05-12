using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp4.Abstractions;
using WebApp4.DTOs.UserDTOs;
using WebApp4.Entities.Identities;
using WebApp4.Models;

namespace WebApp4.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers() => await _userService.GetAllUseAsyncr() is GenericResponseModel<List<GetUserDTO>> users ? StatusCode(users.StatusCode, users) : NotFound();
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id) => await _userService.GetById(id) is GenericResponseModel<GetUserDTO> user ? StatusCode(user.StatusCode, user) : NotFound();
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDTO userDTO) => await _userService.CreateAsync(userDTO) is GenericResponseModel<CreateUserResponseDTO> user ? StatusCode(user.StatusCode, user) : NotFound();
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO userDTO) => await _userService.UpdateUserAsync(userDTO) is GenericResponseModel<bool> result ? StatusCode(result.StatusCode, result) : NotFound();
        [HttpDelete("{userIdOrName}")]
        public async Task<IActionResult> DeleteUser(string userIdOrName) => await _userService.DeleteUserAsync(userIdOrName) is GenericResponseModel<bool> result ? StatusCode(result.StatusCode, result) : NotFound();
        [HttpGet("{userIdOrName}")]
        public async Task<IActionResult> GetRolesToUser(string userIdOrName) => await _userService.GetRolesToUserAsync(userIdOrName) is GenericResponseModel<string[]> roles ? StatusCode(roles.StatusCode, roles) : NotFound();
        [HttpPost("{userId}")]
        public async Task<IActionResult> AssignRoleToUser(string userId, string[] role) => await _userService.AssignRoleToUserAsync(userId, role) is GenericResponseModel<bool> result ? StatusCode(result.StatusCode, role) : NotFound();
    }
}
