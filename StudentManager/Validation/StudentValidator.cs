using StudentManager.Models;
using System.Net.Mail;

namespace StudentManager.Validation
{
    internal static class StudentValidator
    {
        public static ValidationResult ValidateName(string? name, string field)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ValidationResult.Invalid($"{field} cannot be empty.");
            }

            if (!name.All(character =>
                char.IsLetter(character) || " -'".Contains(character)))
            {
                return ValidationResult.Invalid(
                    $"{field} cannot contain invalid characters.");
            }

            return ValidationResult.Valid();
        }

        public static ValidationResult ValidateYear(int year, Course? course)
        {
            if (course is null)
            {
                return ValidationResult.Invalid("Course cannot be null.");
            }

            if (year < 1 || year > course.Duration)
            {
                return ValidationResult.Invalid(
                    "Year of study must be within the course duration.");
            }

            return ValidationResult.Valid();
        }

        public static ValidationResult ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return ValidationResult.Invalid("Email is required.");
            }

            if (!MailAddress.TryCreate(email, out var address))
            {
                return ValidationResult.Invalid("Email format is not valid.");
            }

            if (address.Address != email)
            {
                return ValidationResult.Invalid("Email format is not valid.");
            }

            return ValidationResult.Valid();
        }

        public static ValidationResult ValidateDateOfBirth(DateOnly dateOfBirth)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            if (dateOfBirth > today)
            {
                return ValidationResult.Invalid(
                    "Date of birth cannot be in the future.");
            }

            if (dateOfBirth < today.AddYears(-100))
            {
                return ValidationResult.Invalid(
                    "Date of birth is not plausible.");
            }

            return ValidationResult.Valid();
        }

        public static ValidationResult ValidateMark(int mark)
        {
            if (mark < 0 || mark > 100)
            {
                return ValidationResult.Invalid(
                    "Mark must be between 0 and 100.");
            }

            return ValidationResult.Valid();
        }
    }
}