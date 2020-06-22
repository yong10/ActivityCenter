using System;
using System.ComponentModel.DataAnnotations;

namespace CsharpBelt.Models
{
    public class ValAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            if ((DateTime) value < DateTime.Now)
            {
                return new ValidationResult("Date must be in the future!");
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}