using Microsoft.Data.SqlClient;
using SchedulingSystemAPI.Models;
using SchedulingSystemAPI.Services.Interfaces;

namespace SchedulingSystemAPI.Services
{
    public class ScheduleSystemDatabase : IScheduleSystemDatabase
    {
            private readonly string _connectionString;

            public ScheduleSystemDatabase(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            public SqlConnection GetConnection()
            {
                return new SqlConnection(_connectionString);
            }

            public void InsertEmployees(List<Employee> employees)
            {
                using var connection = GetConnection();
                connection.Open();

                foreach (var employee in employees)
                {
                    var command = new SqlCommand(@"
                    INSERT INTO Employee (Id, Name, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday)
                    VALUES (@Id, @Name, @Monday, @Tuesday, @Wednesday, @Thursday, @Friday, @Saturday, @Sunday)", connection);

                    command.Parameters.AddWithValue("@Id", employee.Id);
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@Monday", employee.Availability.Monday);
                    command.Parameters.AddWithValue("@Tuesday", employee.Availability.Tuesday);
                    command.Parameters.AddWithValue("@Wednesday", employee.Availability.Wednesday);
                    command.Parameters.AddWithValue("@Thursday", employee.Availability.Thursday);
                    command.Parameters.AddWithValue("@Friday", employee.Availability.Friday);
                    command.Parameters.AddWithValue("@Saturday", employee.Availability.Saturday);
                    command.Parameters.AddWithValue("@Sunday", employee.Availability.Sunday);

                    command.ExecuteNonQuery();
                }
            }

            public void InsertShifts(List<Shift> shifts)
            {
                using var connection = GetConnection();
                connection.Open();

                foreach (var shift in shifts)
                {
                    var command = new SqlCommand("INSERT INTO Shift (ScheduledWorkDay) VALUES (@Day)", connection);
                    command.Parameters.AddWithValue("@Day", shift.ScheduledWorkDay);
                    command.ExecuteNonQuery();
                }
            }

            public List<Employee> GetEmployees()
            {
                var employees = new List<Employee>();

                using var connection = GetConnection();
                connection.Open();

                var command = new SqlCommand("SELECT * FROM Employee", connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Availability = new Availability
                        {
                            Monday = reader.GetBoolean(2),
                            Tuesday = reader.GetBoolean(3),
                            Wednesday = reader.GetBoolean(4),
                            Thursday = reader.GetBoolean(5),
                            Friday = reader.GetBoolean(6),
                            Saturday = reader.GetBoolean(7),
                            Sunday = reader.GetBoolean(8)
                        }
                    });
                }

                return employees;
            }

            public List<Shift> GetShifts()
            {
                var shifts = new List<Shift>();

                using var connection = GetConnection();
                connection.Open();

                var command = new SqlCommand("SELECT * FROM Shift", connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    shifts.Add(new Shift
                    {
                        Id = reader.GetInt32(0),
                        ScheduledWorkDay = reader.GetString(1)
                    });
                }

                return shifts;
            }

            public void InsertSchedule(int employeeId, int shiftId)
            {
                using var connection = GetConnection();
                connection.Open();

                // Check if the shiftId is already assigned
                var checkShiftCommand = new SqlCommand("SELECT COUNT(*) FROM Schedule WHERE ShiftId = @ShiftId", connection);
                checkShiftCommand.Parameters.AddWithValue("@ShiftId", shiftId);

                var shiftCount = (int)checkShiftCommand.ExecuteScalar();

                if (shiftCount > 0)
                {
                    // If the shift is already assigned, do not insert and log a message
                    Console.WriteLine($"ShiftId {shiftId} is already assigned.");
                    return;
                }


                var command = new SqlCommand("INSERT INTO Schedule (EmployeeId, ShiftId) VALUES (@EmployeeId, @ShiftId)", connection);
                command.Parameters.AddWithValue("@EmployeeId", employeeId);
                command.Parameters.AddWithValue("@ShiftId", shiftId);
                command.ExecuteNonQuery();
            }

            public List<Schedule> GetSchedule()
            {
                var schedules = new List<Schedule>();

                using var connection = GetConnection();
                connection.Open();

                var command = new SqlCommand("SELECT * FROM Schedule", connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    schedules.Add(new Schedule
                    {
                        Id = reader.GetInt32(0),
                        EmployeeId = reader.GetInt32(1),
                        ShiftId = reader.GetInt32(2)
                    });
                }

                return schedules;
            }
        }
    }
