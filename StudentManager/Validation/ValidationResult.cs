using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManager.Validation
{
    /*
     * Represents the outcome of a validation operation.
     * A result indicates whether the input is valid and, if not,
     * provides an error message explaining why validation failed.
     */
    internal record ValidationResult (
        bool IsValid,
        string? Error = null)
    {
        public static ValidationResult Valid()
        {
            return new ValidationResult(true);
        }

        public static ValidationResult Invalid(string errorMessage)
        {
            return new ValidationResult(false, errorMessage);
        }
    }
}
