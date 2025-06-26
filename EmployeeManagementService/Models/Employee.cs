namespace EmployeeManagementService.Models
{
    public class Employee
    {
        public string EmployeeId { get; set; } = Guid.NewGuid().ToString();
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Department { get; set; }
        public required DateTime HireDate { get; set; }
    }
}