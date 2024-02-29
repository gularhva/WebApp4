using Microsoft.AspNetCore.Mvc;
using WebApp4.Abstractions;
using WebApp4.DTOs.Student_DTOs;
using WebApp4.Models;

namespace WebApp4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public WeatherForecastController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet]
        public Task<GenericResponseModel<List<StudentGetDTO>>> Get()
        {
            var data = _studentService.GetAllStudents();
            return data;
        }
        [HttpPost]
        public Task<GenericResponseModel<StudentCreateDTO>> AddStudent(StudentCreateDTO model)
        {
            return _studentService.AddStudent(model);
        }
    }
}