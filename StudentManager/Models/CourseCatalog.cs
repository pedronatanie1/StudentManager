namespace StudentManager.Models
{
    internal static class CourseCatalogue
    {
        public static IReadOnlyList<Course> Courses { get; } =
        [
            new Course("CS", "Computer Science", Department.Computing, 3),
            new Course("SE", "Software Engineering", Department.Computing, 3),
            new Course("CY", "Cyber Security", Department.Computing, 3),
            new Course("DS", "Data Science", Department.Computing, 3)
        ];
    }
}
