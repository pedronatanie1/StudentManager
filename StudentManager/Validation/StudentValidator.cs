using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
