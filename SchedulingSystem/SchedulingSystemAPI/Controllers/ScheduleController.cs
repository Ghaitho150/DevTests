using Microsoft.AspNetCore.Mvc;
using SchedulingSystemAPI.Models;
using SchedulingSystemAPI.Services.Interfaces;

namespace SchedulingSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController : Controller
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IScheduleSystemDatabase _scheduleSystemDatabase;

        public ScheduleController(ILogger<ScheduleController> logger, IScheduleSystemDatabase scheduleDatabase)
        {
            _logger = logger;
            _scheduleSystemDatabase = scheduleDatabase;
        }

        [HttpPost("create")]
        public IActionResult CreateSchedule()
        {
            try
            {
                var employees = _scheduleSystemDatabase.GetEmployees();
                var shifts = _scheduleSystemDatabase.GetShifts();


                // Dictionary to keep track of how many shifts each employee has been assigned
                var employeeShiftCount = new Dictionary<int, int>();
                foreach (var employee in employees)
                {
                    employeeShiftCount[employee.Id] = 0; // Initialize to 0 shifts
                }


                foreach (var shift in shifts)
                {
                    // Get available employees for the current day
                    var availableEmployees = employees.Where(e => CanWorkOnDay(e, shift.ScheduledWorkDay.Trim()) && employeeShiftCount[e.Id] < shifts.Count / employees.Count).ToList();

                    // Sort available employees by the number of shifts assigned (ascending) to balance the workload
                    var employeeToAssign = availableEmployees.OrderBy(e => employeeShiftCount[e.Id]).FirstOrDefault();
                    if ((employeeToAssign != null))
                    {
                        _scheduleSystemDatabase.InsertSchedule(employeeToAssign.Id, shift.Id);
                        employeeShiftCount[employeeToAssign.Id]++;
                    }
                    

                    /*foreach (var employee in employees)
                    {
                        if (CanWorkOnDay(employee, shift.ScheduledWorkDay.Trim()))
                        {
                            _scheduleSystemDatabase.InsertSchedule(employee.Id, shift.Id);
                            break;
                        }
                    }*/
                }

                return Ok(new { message = "Schedule created successfully" });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        [HttpGet]
        public IActionResult GetSchedule()
        {
            var schedule = _scheduleSystemDatabase.GetSchedule();
            return Ok(schedule);
        }

        private bool CanWorkOnDay(Employee employee, string day)
        {
            return day switch
            {
                "Monday" => employee.Availability.Monday,
                "Tuesday" => employee.Availability.Tuesday,
                "Wednesday" => employee.Availability.Wednesday,
                "Thursday" => employee.Availability.Thursday,
                "Friday" => employee.Availability.Friday,
                "Saturday" => employee.Availability.Saturday,
                "Sunday" => employee.Availability.Sunday,
                _ => false
            };
        }
    }
}

