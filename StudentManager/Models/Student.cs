using System;
using System.Collections.Generic;
using System.Text;

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
                throw new ArgumentOutOfRangeException(nameof(id));
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("First name is required.", nameof(lastName));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("First name is required.", nameof(email));

            var today = DateOnly.FromDateTime(DateTime.Today);

            if (birth > today)
                throw new ArgumentOutOfRangeException(nameof(birth),
                    "Date of birth cannot be in the future.");

            if (birth < today.AddYears(-100))
                throw new ArgumentOutOfRangeException(nameof(birth),
                    "Date of birth is not plausible.");

            if (course is null)
                throw new ArgumentNullException(nameof(course));
            if (year <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

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
