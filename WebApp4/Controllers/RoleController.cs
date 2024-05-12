using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp4.Abstractions;
using WebApp4.Implementations.Services;

namespace WebApp4.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService= roleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var response = await _roleService.GetAllRoles();
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var response = await _roleService.GetRoleById(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            var response = await _roleService.CreateRole(name);
            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var response = await _roleService.DeleteRole(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateRole(string id,string name)
        {
            var response = await _roleService.UpdateRole(id,name);
            return StatusCode(response.StatusCode, response);
        }
    }
}
