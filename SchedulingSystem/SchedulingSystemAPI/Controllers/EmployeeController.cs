using Microsoft.AspNetCore.Mvc;
using SchedulingSystemAPI.Models;
using SchedulingSystemAPI.Services.Interfaces;

namespace SchedulingSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IScheduleSystemDatabase _scheduleSystemDatabase;

        public EmployeeController(ILogger<EmployeeController> logger, IScheduleSystemDatabase scheduleDatabase)
        {
            _logger = logger;
            _scheduleSystemDatabase = scheduleDatabase;
        }

        [HttpPost("insert")]
        public IActionResult InsertEmployees([FromBody] List<Employee> employees)
        {
            try
            {
                _scheduleSystemDatabase.InsertEmployees(employees);
                return Ok(new { message = "Employees inserted successfully" });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
            
        }
    }
}
