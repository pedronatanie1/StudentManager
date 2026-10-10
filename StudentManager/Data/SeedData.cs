using StudentManager.Models;
using StudentManager.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManager.Data
{
    internal static class SeedData
    {
        public static void Load(StudentService service)
        {
            ArgumentNullException.ThrowIfNull(service);

            Course computerScience = CourseCatalogue.Courses[0];
            Course softwareEngineering = CourseCatalogue.Courses[1];
            Course cyberSecurity = CourseCatalogue.Courses[2];
            Course dataScience = CourseCatalogue.Courses[3];

            Student[] students =
            [
                new Student(
                    1001,
                    "Alice",
                    "Johnson",
                    "alice@example.com",
                    new DateOnly(2005, 3, 12),
                    computerScience,
                    1),

                new Student(
                    1002,
                    "Daniel",
                    "Smith",
                    "daniel@example.com",
                    new DateOnly(2004, 7, 24),
                    softwareEngineering,
                    2),

                new Student(
                    1003,
                    "Maria",
                    "Santos",
                    "maria@example.com",
                    new DateOnly(2005, 11, 8),
                    computerScience,
                    1),

                new Student(
                    1004,
                    "Pedro",
                    "Silva",
                    "pedro.silva@example.com",
                    new DateOnly(2003, 1, 30),
                    cyberSecurity,
                    3),

                new Student(
                    1005,
                    "Sofia",
                    "Pereira",
                    "sofia@example.com",
                    new DateOnly(2004, 9, 2),
                    dataScience,
                    2),

                new Student(
                    1006,
                    "Liam",
                    "Walker",
                    "liam@example.com",
                    new DateOnly(2005, 5, 19),
                    softwareEngineering,
                    1),

                new Student(
                    1007,
                    "Aisha",
                    "Khan",
                    "aisha@example.com",
                    new DateOnly(2002, 12, 3),
                    computerScience,
                    3),

                new Student(
                    1008,
                    "Tom",
                    "Brown",
                    "tom@example.com",
                    new DateOnly(2004, 2, 14),
                    cyberSecurity,
                    2)
            ];

            foreach (Student student in students)
            {
                service.AddStudent(student);
            }

            // A mix of statuses, so filtering and statistics have something to show
            students[3].ChangeStatus(StudentStatus.Suspended);
            students[6].ChangeStatus(StudentStatus.Graduated);
            students[7].ChangeStatus(StudentStatus.Withdrawn);

            // A mix of marks, including a fail, so performance figures are interesting
            Module programming = ModuleCatalogue.Modules[0];
            Module databases = ModuleCatalogue.Modules[1];
            Module algorithms = ModuleCatalogue.Modules[2];
            Module webDevelopment = ModuleCatalogue.Modules[3];

            students[0].RecordGrade(programming, 78);
            students[0].RecordGrade(databases, 65);

            students[1].RecordGrade(programming, 32);
            students[1].RecordGrade(algorithms, 84);

            students[4].RecordGrade(databases, 71);
            students[4].RecordGrade(algorithms, 58);
            students[4].RecordGrade(webDevelopment, 90);

            students[6].RecordGrade(programming, 82);
            students[6].RecordGrade(databases, 77);
            students[6].RecordGrade(algorithms, 69);
            students[6].RecordGrade(webDevelopment, 74);
        }
    }
}
