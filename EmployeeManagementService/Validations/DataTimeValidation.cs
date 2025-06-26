
using System.ComponentModel.DataAnnotations;
namespace EmployeeManagementService.Validations;
public class CustomHireDateValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime hireDate)
        {
            if (hireDate > DateTime.Today)
            {
                return new ValidationResult("Hire Date cannot be in the future.");
            }
        }
        return ValidationResult.Success!;
    }
}