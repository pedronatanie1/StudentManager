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
