using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementService.Models;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagementService.Service.Jwt
{
    public interface IJwtService
    {
            string GenerateJwtToken(Users user);
    }
}