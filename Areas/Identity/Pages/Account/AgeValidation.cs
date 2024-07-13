using System.ComponentModel.DataAnnotations;

namespace Dw23787.Areas.Identity.Pages.Account
{
    public class AgeValidation : ValidationAttribute
    {
        private readonly int _minimumAge;

        public AgeValidation(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Birthdate is required");
            }

            DateOnly birthDate = (DateOnly)value;
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthDate.Year;

            if (birthDate.AddYears(age) > today)
            {
                age--;
            }

            if (age < _minimumAge)
            {
                return new ValidationResult($"You must be at least {_minimumAge} years old.");
            }

            return ValidationResult.Success;
        }
    }
}
