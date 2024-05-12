using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp4.Abstractions;
using WebApp4.DTOs.School_DTOs;

namespace WebApp4.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;
        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin", Roles = "Admin,User")]
        public async Task<IActionResult> GetAllSchools()
        {
            var response = await _schoolService.GetAllSchools();
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Admin,User")]
        [HttpGet("get_by_id")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _schoolService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("add_school")]
        public async Task<IActionResult> AddSchool(SchoolCreateDTO model)
        {
            var response = await _schoolService.AddSchool(model);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSchool(int id)
        {
            var response = await _schoolService.DeleteSchool(id);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateSchool(SchoolUpdateDTO model)
        {
            var response = await _schoolService.UpdateSchool(model);
            return StatusCode(response.StatusCode, response);
        }
    }
}
