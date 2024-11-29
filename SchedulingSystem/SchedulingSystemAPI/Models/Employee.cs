namespace SchedulingSystemAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public required Availability Availability { get; set; }

        public Employee()
        {
            Availability = new Availability();
        }
    }
}


