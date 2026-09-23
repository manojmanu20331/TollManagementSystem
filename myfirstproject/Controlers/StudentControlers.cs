using Microsoft.AspNetCore.Mvc;
using MyFirstProject.Models;

namespace MyFirstProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            Student student = new Student();

            student.RollNumber = 101;
            student.Name = "Raju Kumar";

            return Ok(student);
        }
    }
}