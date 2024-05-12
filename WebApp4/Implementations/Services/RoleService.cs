using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp4.Abstractions;
using WebApp4.Entities.Identities;
using WebApp4.Models;

namespace WebApp4.Implementations.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<GenericResponseModel<bool>> CreateRole(string name)
        {
            GenericResponseModel<bool> response = new GenericResponseModel<bool>() { Data = false, StatusCode = 400 };
            if (name == null)
            {
                return response;
            }
            AppRole role = new AppRole();
            role.Name = name;
            role.Id = Guid.NewGuid().ToString();
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return response;
            }
            response.StatusCode = 200;
            response.Data = true;
            return response;
        }

        public async Task<GenericResponseModel<bool>> DeleteRole(string id)
        {
            GenericResponseModel<bool> response = new GenericResponseModel<bool>() { Data = false, StatusCode = 400 };
            if (id == null)
            {
                return response;
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return response;
            }
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                return response;
            }
            response.StatusCode = 200;
            response.Data = true;
            return response;
        }

        public async Task<GenericResponseModel<object>> GetAllRoles()
        {
            GenericResponseModel<object> response = new GenericResponseModel<object>() { Data = null, StatusCode = 400 };
            var roles = await _roleManager.Roles.ToListAsync();
            if (roles == null)
            {
                return response;
            }
            response.StatusCode = 200;
            response.Data = roles;
            return response;
        }

        public async Task<GenericResponseModel<object>> GetRoleById(string id)
        {
            GenericResponseModel<object> response = new GenericResponseModel<object>() { Data = null, StatusCode = 400 };
            if (id == null)
                return response;
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return response;
            response.StatusCode = 200;
            response.Data = role;
            return response;
        }

        public async Task<GenericResponseModel<bool>> UpdateRole(string id, string name)
        {
            GenericResponseModel<bool> response = new GenericResponseModel<bool>() { Data = false, StatusCode = 400 };
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return response;
            role.Name = name;
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                return response;
            response.StatusCode = 200;
            response.Data = true;
            return response;
        }
    }
}
