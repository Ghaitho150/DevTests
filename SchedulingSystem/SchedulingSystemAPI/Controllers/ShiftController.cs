using Microsoft.AspNetCore.Mvc;
using SchedulingSystemAPI.Models;
using SchedulingSystemAPI.Services.Interfaces;

namespace SchedulingSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShiftController : Controller
    {
        private readonly ILogger<ShiftController> _logger;
        private readonly IScheduleSystemDatabase _scheduleSystemDatabase;

        public ShiftController(ILogger<ShiftController> logger, IScheduleSystemDatabase scheduleDatabase)
        {
            _logger = logger;
            _scheduleSystemDatabase = scheduleDatabase;
        }
        [HttpPost("insert")]
        public IActionResult InsertShifts([FromBody] List<Shift> shifts)
        {
            try
            {
                _scheduleSystemDatabase.InsertShifts(shifts);
                return Ok(new { message = "Shifts inserted successfully" });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }

        }
    }
}
