using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CsharpBelt.Models
{
    public class StrongPwdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            var reg = new Regex(@"^(?=.*?\d)(?=.*?[A-Z])(?=.*?[a-z])[A-Za-z\d,!@#$%^&*+=]{8,}$");
            if (!reg.IsMatch((string)value))
            {
                return new ValidationResult("Password must have at least 1 number, 1 upper letter, 1 lower letter, and 1 special character!");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}