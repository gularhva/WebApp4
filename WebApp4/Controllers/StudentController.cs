using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp4.Abstractions;
using WebApp4.DTOs.Student_DTOs;

namespace WebApp4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet("get_all_students")]
        public async Task<IActionResult> GetAllStudents()
        {
            var response =await _studentService.GetAllStudents();
            return StatusCode(response.StatusCode,response);
        }
        [HttpGet("get_all_students_by_schoolid")]
        public async Task<IActionResult> GetAllStudentsBySchoolId(int id)
        {
            var response = await _studentService.GetAllStudentsBySchoolId(id);
            return StatusCode(response.StatusCode,response);
        }
        [HttpGet("get_by_id")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _studentService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost("add_student")]
        public async Task<IActionResult> AddStudent(StudentCreateDTO model)
        {
            var response = await _studentService.AddStudent(model);
            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var response = await _studentService.DeleteStudent(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut("update_student")]
        public async Task<IActionResult> UpdateStudent(StudentUpdateDTO model)
        {
            var response = await _studentService.UpdateStudent(model);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut("change_school")]
        public async Task<IActionResult> ChangeSchool(ChangeSchoolDTO model)
        {
            var response = await _studentService.ChangeSchool(model);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut("change_school_by_id")]
        public async Task<IActionResult> ChangeSchool(int studentId, int newSchoolId)
        {
            var response = await _studentService.ChangeSchool(studentId,newSchoolId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
