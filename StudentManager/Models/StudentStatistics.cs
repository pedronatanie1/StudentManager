namespace StudentManager.Models
{
    internal class StudentStatistics
    {
        public int TotalStudents { get; init; }

        public Dictionary<StudentStatus, int> StudentsByStatus { get; init; }
            = new();

        public Dictionary<string, int> StudentsByCourse { get; init; }
            = new();
    }
}
