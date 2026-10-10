using System.Net.Mail;

namespace StudentManager.Models
{
    internal class Student
    {
        public int Id { get; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public DateOnly DateOfBirth { get; }
        public Course Course { get; private set; }
        public int YearOfStudy { get; private set; }
        public StudentStatus Status { get; private set; } = StudentStatus.Active;

        public Student(int id, string firstName, string lastName,
            string email, DateOnly birth, Course course, int year)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Student ID must be greater than zero.");
            }


            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException(
                    "First name is required.",
                    nameof(firstName));
            }


            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException(
                    "Last name is required.",
                    nameof(lastName));
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException(
                    "Email is required.",
                    nameof(email));
            }

            if (!MailAddress.TryCreate(email, out MailAddress? address)
                || address.Address != email)
            {
                throw new ArgumentException(
                    "Email format is not valid.",
                    nameof(email));
            }

            var today = DateOnly.FromDateTime(DateTime.Today);

            if (birth > today)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(birth),
                    "Date of birth cannot be in the future.");
            }

            if (birth < today.AddYears(-100))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(birth),
                    "Date of birth is not plausible.");
            }


            if (course is null)
            {
                throw new ArgumentNullException(
                    nameof(course));
            }

            if (year < 1 || year > course.Duration)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(year),
                    "Year of study must be within the course duration.");
            }

            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateOfBirth = birth;
            Course = course;
            YearOfStudy = year;
        }

        // Changing state: only these methods may modify a student after creation
        public void ChangeStatus(StudentStatus newStatus)
        {
            if (!Enum.IsDefined(newStatus))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(newStatus),
                    "Unknown student status.");
            }

            Status = newStatus;
        }

        public void ChangeYear(int newYear)
        {
            if (newYear < 1 || newYear > Course.Duration)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(newYear),
                    "Year of study must be within the course duration.");
            }

            YearOfStudy = newYear;
        }

        // The year is passed in too, because the old year may not exist on the new course
        public void ChangeCourse(Course newCourse, int newYear)
        {
            ArgumentNullException.ThrowIfNull(newCourse);

            if (newYear < 1 || newYear > newCourse.Duration)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(newYear),
                    "Year of study must be within the course duration.");
            }

            Course = newCourse;
            YearOfStudy = newYear;
        }

        // Academic performance
        public const int PassMark = 40;

        private readonly List<ModuleResult> _results = new();

        public IReadOnlyList<ModuleResult> Results => _results.AsReadOnly();

        public void RecordGrade(Module module, int mark)
        {
            ArgumentNullException.ThrowIfNull(module);

            if (mark < 0 || mark > 100)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(mark),
                    "Mark must be between 0 and 100.");
            }

            // Replace an existing result for this module
            _results.RemoveAll(r => r.Module.Id == module.Id);

            _results.Add(new ModuleResult(module, mark));
        }

        public double? AverageMark =>
            _results.Count == 0
                ? null
                : _results.Average(r => r.Mark);

        public int? HighestMark =>
            _results.Count == 0
                ? null
                : _results.Max(r => r.Mark);

        public int? LowestMark =>
            _results.Count == 0
                ? null
                : _results.Min(r => r.Mark);

        public int PassedModules =>
            _results.Count(r => r.Mark >= PassMark);

        public int FailedModules =>
            _results.Count(r => r.Mark < PassMark);
    }
}
