using StudentManager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace StudentManager.Validation
{
    internal static class StudentValidator
    {
        public static ValidationResult ValidateName(string? name, string field)
        {
            if (string.IsNullOrWhiteSpace(name))  
                return ValidationResult.Invalid($"{field} cannot be empty");

            if (!name.All(character => char.IsLetter(character) || " -'".Contains(character)))
                return ValidationResult.Invalid($"{field} cannot contain invalid characters");

            return ValidationResult.Valid();
        }

        public static ValidationResult ValidateYear(int year, Course? course)
        {
            if (course is null)
                return ValidationResult.Invalid("Course cannot be null.");

            if (year < 1 || year > course.Duration)
                return ValidationResult.Invalid("Year cannot be outside of course's duration scope");

            return ValidationResult.Valid();
        }
    }
}
