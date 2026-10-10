using StudentManager.Models;
using StudentManager.Services;
using StudentManager.Validation;

namespace StudentManager.UI
{
    /*
    * The console user interface. It is the only class that reads from or
    * writes to the console. All the real work is delegated to StudentService,
    * and all input checking is delegated to StudentValidator.
    */
    internal class MainMenu
    {
        // One menu entry: the text shown to the user and the method that runs
        private sealed record MenuItem(string Label, Action Run);

        private readonly StudentService _service;
        private readonly Dictionary<int, MenuItem> _actions;
        private bool _running = true;

        public MainMenu(StudentService service)
        {
            ArgumentNullException.ThrowIfNull(service);

            _service = service;

            _actions = new Dictionary<int, MenuItem>
            {
                [1] = new MenuItem("Add student", AddStudent),
                [2] = new MenuItem("View students", ViewStudents),
                [3] = new MenuItem("View student details", ViewStudentDetails),
                [4] = new MenuItem("Search students", SearchStudents),
                [5] = new MenuItem("Filter students", FilterStudents),
                [6] = new MenuItem("Update student", UpdateStudent),
                [7] = new MenuItem("Remove student", RemoveStudent),
                [8] = new MenuItem("Manage modules", ManageModules),
                [9] = new MenuItem("Record grade", RecordGrade),
                [10] = new MenuItem("Student statistics", ShowStatistics),
                [11] = new MenuItem("Exit", () => _running = false)
            };
        }

        public void Run()
        {
            while (_running)
            {
                ShowMenu();

                int choice = ConsoleInput.ReadInt("Select an option: ");

                if (_actions.TryGetValue(choice, out MenuItem? item))
                {
                    item.Run();
                }
                else
                {
                    Console.WriteLine("Unknown option.");
                }
            }

            Console.WriteLine("Goodbye.");
        }

        private void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("================================");
            Console.WriteLine("       STUDENT MANAGER");
            Console.WriteLine("================================");

            foreach (KeyValuePair<int, MenuItem> entry in _actions.OrderBy(a => a.Key))
            {
                Console.WriteLine($"{entry.Key,2}. {entry.Value.Label}");
            }

            Console.WriteLine();
        }

        // ---------- Add ----------

        private void AddStudent()
        {
            Header("Add student");

            int id = ReadNewStudentId();

            string firstName = ConsoleInput.ReadValidated(
                "First name: ",
                input => StudentValidator.ValidateName(input, "First name"));

            string lastName = ConsoleInput.ReadValidated(
                "Last name: ",
                input => StudentValidator.ValidateName(input, "Last name"));

            string email = ConsoleInput.ReadValidated(
                "Email: ",
                StudentValidator.ValidateEmail);

            DateOnly dateOfBirth = ReadValidDateOfBirth();
            Course course = ChooseCourse();
            int year = ReadValidYear(course);

            try
            {
                var student = new Student(
                    id, firstName, lastName, email, dateOfBirth, course, year);

                if (_service.AddStudent(student))
                {
                    Console.WriteLine($"Student {firstName} {lastName} added.");
                }
                else
                {
                    Console.WriteLine($"A student with ID {id} already exists.");
                }
            }
            catch (ArgumentException ex)
            {
                // The input was already validated, so reaching this means a bug.
                // The constructor is the safety net.
                Console.WriteLine($"Could not create the student: {ex.Message}");
            }
        }

        private int ReadNewStudentId()
        {
            while (true)
            {
                int id = ConsoleInput.ReadInt("Student ID: ");

                if (id <= 0)
                {
                    Console.WriteLine("Student ID must be greater than zero.");
                }
                else if (_service.FindById(id) is not null)
                {
                    Console.WriteLine($"A student with ID {id} already exists.");
                }
                else
                {
                    return id;
                }
            }
        }

        private static DateOnly ReadValidDateOfBirth()
        {
            while (true)
            {
                DateOnly dateOfBirth = ConsoleInput.ReadDate("Date of birth (dd/mm/yyyy): ");

                ValidationResult result = StudentValidator.ValidateDateOfBirth(dateOfBirth);

                if (result.IsValid)
                {
                    return dateOfBirth;
                }

                Console.WriteLine(result.Error);
            }
        }

        private static int ReadValidYear(Course course)
        {
            while (true)
            {
                int year = ConsoleInput.ReadInt(
                    $"Year of study (1-{course.Duration}): ");

                ValidationResult result = StudentValidator.ValidateYear(year, course);

                if (result.IsValid)
                {
                    return year;
                }

                Console.WriteLine(result.Error);
            }
        }

        // ---------- View, search, filter ----------

        private void ViewStudents()
        {
            Header("View students");

            SortField field = ConsoleInput.ReadOptionalEnum<SortField>(
                $"Sort by ({string.Join(", ", Enum.GetNames<SortField>())}) " +
                "[Enter for Id]: ") ?? SortField.Id;

            bool descending = ConsoleInput.ReadYesNo("Descending order? (y/n): ");

            PrintStudents(_service.Sort(_service.GetAll(), field, descending));
        }

        private void ViewStudentDetails()
        {
            Header("Student details");

            Student? student = AskForExistingStudent("Student ID: ");

            if (student is null)
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"ID:            {student.Id}");
            Console.WriteLine($"Name:          {student.FirstName} {student.LastName}");
            Console.WriteLine($"Email:         {student.Email}");
            Console.WriteLine($"Date of birth: {student.DateOfBirth:dd/MM/yyyy}");
            Console.WriteLine($"Course:        {student.Course.CourseName} ({student.Course.CourseId})");
            Console.WriteLine($"Year of study: {student.YearOfStudy}");
            Console.WriteLine($"Status:        {student.Status}");
        }

        private void SearchStudents()
        {
            Header("Search students");

            string term = ConsoleInput.ReadNonEmptyString(
                "Search term (ID, name or email): ");

            PrintStudents(_service.Search(term));
        }

        private void FilterStudents()
        {
            Header("Filter students");

            Course? course = ChooseCourseOrAny();

            int? year = ConsoleInput.ReadOptionalInt(
                "Year of study [Enter for any]: ");

            StudentStatus? status = ConsoleInput.ReadOptionalEnum<StudentStatus>(
                $"Status ({string.Join(", ", Enum.GetNames<StudentStatus>())}) " +
                "[Enter for any]: ");

            PrintStudents(_service.Filter(course, year, status));
        }

        // ---------- Update and remove ----------

        private void UpdateStudent()
        {
            Header("Update student");

            Student? student = AskForExistingStudent("Student ID to update: ");

            if (student is null)
            {
                return;
            }

            Console.WriteLine($"Updating {student.FirstName} {student.LastName}");
            Console.WriteLine("1. Change course");
            Console.WriteLine("2. Change year of study");
            Console.WriteLine("3. Change status");

            int choice = ConsoleInput.ReadInt("Choose: ");

            switch (choice)
            {
                case 1:
                    {
                        Course newCourse = ChooseCourse();
                        int newYear = ReadValidYear(newCourse);
                        student.ChangeCourse(newCourse, newYear);
                        Console.WriteLine("Course updated.");
                        break;
                    }

                case 2:
                    {
                        int newYear = ReadValidYear(student.Course);
                        student.ChangeYear(newYear);
                        Console.WriteLine("Year of study updated.");
                        break;
                    }

                case 3:
                    {
                        StudentStatus newStatus = ConsoleInput.ReadEnum<StudentStatus>(
                            $"New status ({string.Join(", ", Enum.GetNames<StudentStatus>())}): ");
                        student.ChangeStatus(newStatus);
                        Console.WriteLine("Status updated.");
                        break;
                    }

                default:
                    Console.WriteLine("Unknown option.");
                    break;
            }
        }

        private void RemoveStudent()
        {
            Header("Remove student");

            Student? student = AskForExistingStudent("Student ID to remove: ");

            if (student is null)
            {
                return;
            }

            bool confirmed = ConsoleInput.ReadYesNo(
                $"Remove {student.FirstName} {student.LastName}? (y/n): ");

            if (!confirmed)
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            if (_service.RemoveStudent(student.Id))
            {
                Console.WriteLine("Student removed.");
            }
            else
            {
                Console.WriteLine("Student could not be removed.");
            }
        }

        // ---------- Modules and grades ----------

        private void ManageModules()
        {
            Header("Manage modules");

            Console.WriteLine("1. View all modules");
            Console.WriteLine("2. View a student's modules and results");

            int choice = ConsoleInput.ReadInt("Choose: ");

            switch (choice)
            {
                case 1:
                    ShowModules();
                    break;

                case 2:
                    ShowStudentResults();
                    break;

                default:
                    Console.WriteLine("Unknown option.");
                    break;
            }
        }

        private static void ShowModules()
        {
            Console.WriteLine();
            Console.WriteLine("ID".PadRight(8) + "Module".PadRight(22) + "Credits");

            foreach (Module module in ModuleCatalogue.Modules)
            {
                Console.WriteLine($"{module.Id,-8}{module.Name,-22}{module.Credits}");
            }
        }

        private void RecordGrade()
        {
            Header("Record grade");

            Student? student = AskForExistingStudent("Student ID: ");

            if (student is null)
            {
                return;
            }

            Module module = ChooseModule();
            int mark = ReadValidMark();

            if (_service.RecordGrade(student.Id, module, mark))
            {
                Console.WriteLine(
                    $"Recorded {mark} in {module.Name} for " +
                    $"{student.FirstName} {student.LastName}.");

                if (student.AverageMark is double average)
                {
                    Console.WriteLine($"Current average: {average:F1}");
                }
            }
            else
            {
                Console.WriteLine("The grade could not be recorded.");
            }
        }

        private void ShowStudentResults()
        {
            Student? student = AskForExistingStudent("Student ID: ");

            if (student is null)
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine(
                $"Results for {student.FirstName} {student.LastName} " +
                $"({student.Course.CourseName}, year {student.YearOfStudy})");

            if (student.Results.Count == 0)
            {
                Console.WriteLine("No grades recorded yet.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("ID".PadRight(8) + "Module".PadRight(22) + "Mark".PadRight(8) + "Result");

            foreach (ModuleResult result in student.Results.OrderBy(r => r.Module.Id))
            {
                string outcome = result.Mark >= Student.PassMark ? "Pass" : "Fail";

                Console.WriteLine(
                    $"{result.Module.Id,-8}{result.Module.Name,-22}{result.Mark,-8}{outcome}");
            }

            Console.WriteLine();
            Console.WriteLine($"Average:        {student.AverageMark ?? 0:F1}");
            Console.WriteLine($"Highest mark:   {student.HighestMark}");
            Console.WriteLine($"Lowest mark:    {student.LowestMark}");
            Console.WriteLine($"Modules passed: {student.PassedModules}");
            Console.WriteLine($"Modules failed: {student.FailedModules}");
        }

        private static Module ChooseModule()
        {
            for (int i = 0; i < ModuleCatalogue.Modules.Count; i++)
            {
                Module module = ModuleCatalogue.Modules[i];
                Console.WriteLine($"{i + 1}. {module.Id} - {module.Name}");
            }

            while (true)
            {
                int choice = ConsoleInput.ReadInt("Module number: ");

                if (choice >= 1 && choice <= ModuleCatalogue.Modules.Count)
                {
                    return ModuleCatalogue.Modules[choice - 1];
                }

                Console.WriteLine("Please choose a number from the list.");
            }
        }

        private static int ReadValidMark()
        {
            while (true)
            {
                int mark = ConsoleInput.ReadInt("Mark (0-100): ");

                ValidationResult result = StudentValidator.ValidateMark(mark);

                if (result.IsValid)
                {
                    return mark;
                }

                Console.WriteLine(result.Error);
            }
        }

        // ---------- Statistics ----------

        private void ShowStatistics()
        {
            StudentStatistics stats = _service.GetStatistics();

            Console.WriteLine();
            Console.WriteLine("===== Student Statistics =====");
            Console.WriteLine($"Total students: {stats.TotalStudents}");
            Console.WriteLine();

            // Loop over every status, so a status with zero students still shows as 0
            foreach (StudentStatus status in Enum.GetValues<StudentStatus>())
            {
                int count = stats.StudentsByStatus.GetValueOrDefault(status);
                Console.WriteLine($"{status,-12}{count,4}");
            }

            Console.WriteLine();

            foreach (KeyValuePair<string, int> entry in
                stats.StudentsByCourse.OrderByDescending(e => e.Value))
            {
                Console.WriteLine($"{entry.Key,-24}{entry.Value,4}");
            }
        }

        // ---------- Shared helpers ----------

        private static void Header(string title)
        {
            Console.WriteLine();
            Console.WriteLine($"--- {title} ---");
        }

        private Student? AskForExistingStudent(string prompt)
        {
            int id = ConsoleInput.ReadInt(prompt);

            Student? student = _service.FindById(id);

            if (student is null)
            {
                Console.WriteLine($"No student with ID {id}.");
            }

            return student;
        }

        private static void PrintStudents(IEnumerable<Student> students)
        {
            List<Student> list = students.ToList();

            if (list.Count == 0)
            {
                Console.WriteLine("No students found.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine(
                "ID".PadRight(8) + "First name".PadRight(14) + "Last name".PadRight(14)
                + "Course".PadRight(24) + "Year".PadRight(6) + "Status");

            foreach (Student s in list)
            {
                Console.WriteLine(
                    $"{s.Id,-8}{s.FirstName,-14}{s.LastName,-14}" +
                    $"{s.Course.CourseName,-24}{s.YearOfStudy,-6}{s.Status}");
            }

            Console.WriteLine();
            Console.WriteLine($"{list.Count} student(s).");
        }

        private static void ShowCourses()
        {
            for (int i = 0; i < CourseCatalogue.Courses.Count; i++)
            {
                Course course = CourseCatalogue.Courses[i];
                Console.WriteLine($"{i + 1}. {course.CourseName} ({course.Duration} years)");
            }
        }

        private static Course ChooseCourse()
        {
            ShowCourses();

            while (true)
            {
                int choice = ConsoleInput.ReadInt("Course number: ");

                if (choice >= 1 && choice <= CourseCatalogue.Courses.Count)
                {
                    return CourseCatalogue.Courses[choice - 1];
                }

                Console.WriteLine("Please choose a number from the list.");
            }
        }

        private static Course? ChooseCourseOrAny()
        {
            ShowCourses();

            while (true)
            {
                int choice = ConsoleInput.ReadInt("Course number (0 for any): ");

                if (choice == 0)
                {
                    return null;
                }

                if (choice >= 1 && choice <= CourseCatalogue.Courses.Count)
                {
                    return CourseCatalogue.Courses[choice - 1];
                }

                Console.WriteLine("Please choose a number from the list.");
            }
        }
    }
}
