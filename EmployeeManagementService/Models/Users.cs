using Microsoft.AspNetCore.Identity;

namespace EmployeeManagementService.Models
{
    public class Users : IdentityUser
    {
        public override required string Email { get; set; }
        public override required string UserName { get; set; }
    }
}