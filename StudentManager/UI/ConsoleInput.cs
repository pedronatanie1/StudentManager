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
    }
}
