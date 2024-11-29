using Microsoft.Data.SqlClient;
using SchedulingSystemAPI.Models;

namespace SchedulingSystemAPI.Services.Interfaces
{
    public interface IScheduleSystemDatabase
    {
        SqlConnection GetConnection();
        void InsertEmployees(List<Employee> employees);
        void InsertShifts(List<Shift> shifts);
        List<Employee> GetEmployees();
        List<Shift> GetShifts();
        void InsertSchedule(int employeeId, int shiftId);
        List<Schedule> GetSchedule();
    }
}
