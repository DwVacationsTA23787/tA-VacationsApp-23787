using System;
using System.ComponentModel.DataAnnotations;

namespace Dw23787.CustomValidators
{
    public class CustomBirthdateValidator: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {

            DateOnly birthDate = (DateOnly)value;
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthDate.Year;

            if (age < 16)
            {
                return false;
            }

            return true;
        }
    }
}
