using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApp4.Abstractions;
using WebApp4.DTOs.Student_DTOs;
using WebApp4.Models;

namespace WebApp4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public void MyGet()
        {
            Log.Information("salam");
            Log.Error("Error404salam");
        }
    }
}