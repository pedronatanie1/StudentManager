using StudentManager.Validation;

namespace StudentManager.UI
{
    internal static class ConsoleInput
    {
        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid input. Please enter a whole number.");
            }
        }

        // Returns null when the user just presses Enter (used for optional filters)
        public static int? ReadOptionalInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    return null;
                }

                if (int.TryParse(input, out int value))
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid input. Enter a whole number or press Enter to skip.");
            }
        }

        public static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                Console.WriteLine("Input cannot be empty.");
            }
        }

        // Keeps asking until the validator accepts the input
        public static string ReadValidated(
            string prompt,
            Func<string, ValidationResult> validate)
        {
            while (true)
            {
                string input = ReadNonEmptyString(prompt);

                ValidationResult result = validate(input);

                if (result.IsValid)
                {
                    return input;
                }

                Console.WriteLine(result.Error);
            }
        }

        public static DateOnly ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                if (DateOnly.TryParse(
                    Console.ReadLine(),
                    out DateOnly date))
                {
                    return date;
                }

                Console.WriteLine(
                    "Invalid date. Please enter a valid date.");
            }
        }

        public static bool ReadYesNo(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                string? input = Console.ReadLine()?.Trim().ToLowerInvariant();

                if (input is "y" or "yes")
                {
                    return true;
                }

                if (input is "n" or "no")
                {
                    return false;
                }

                Console.WriteLine("Please answer y or n.");
            }
        }

        public static T ReadEnum<T>(string prompt)
            where T : struct, Enum
        {
            while (true)
            {
                Console.Write(prompt);

                string? input = Console.ReadLine();

                if (Enum.TryParse(input, ignoreCase: true, out T value)
                    && Enum.IsDefined(typeof(T), value))
                {
                    return value;
                }

                Console.WriteLine(
                    $"Invalid choice. Valid options are: " +
                    $"{string.Join(", ", Enum.GetNames(typeof(T)))}.");
            }
        }

        // Same as ReadEnum, but pressing Enter returns null (used for optional filters)
        public static T? ReadOptionalEnum<T>(string prompt)
            where T : struct, Enum
        {
            while (true)
            {
                Console.Write(prompt);

                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    return null;
                }

                if (Enum.TryParse(input, ignoreCase: true, out T value)
                    && Enum.IsDefined(typeof(T), value))
                {
                    return value;
                }

                Console.WriteLine(
                    $"Invalid choice. Valid options are: " +
                    $"{string.Join(", ", Enum.GetNames(typeof(T)))}.");
            }
        }
    }
}
