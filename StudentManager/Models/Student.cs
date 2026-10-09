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

            if (!MailAddress.TryCreate(email, out _))
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
    }
}
