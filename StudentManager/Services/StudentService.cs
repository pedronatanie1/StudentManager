using StudentManager.Models;

namespace StudentManager.Services
{
    internal class StudentService 
    {
        // Depend on the storage contract rather than a specific storage implementation.
        private readonly IStudentStorage _storage;
        public StudentService (IStudentStorage storage)
        {
            ArgumentNullException.ThrowIfNull(storage);

            _storage = storage;
        }

        public bool AddStudent(Student student) => _storage.Add(student);
        public Student? FindById(int id) => _storage.GetById(id);
        public IEnumerable<Student> GetAll() => _storage.GetStudents();
        public bool RemoveStudent(int id) => _storage.Remove(id);

        public IEnumerable<Student> Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return Enumerable.Empty<Student>();
            }

            term = term.Trim();

            return _storage.GetStudents().Where(s =>
                s.Id.ToString().Contains(term) ||
                s.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                s.LastName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                s.Email.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        public List<Student> Filter(
            Course? course,
            int? year,
            StudentStatus? status)
        {
            IEnumerable<Student> query = _storage.GetStudents();

            if (course is not null)
            {
                query = query.Where(s => s.Course == course);
            }

            if (year.HasValue)
            {
                query = query.Where(s => s.YearOfStudy == year.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(s => s.Status == status.Value);
            }

            return query.ToList();
        }

        public IEnumerable<Student> Sort(
            IEnumerable<Student> students,
            SortField field,
            bool descending = false)
        {
            Func<Student, object> key = field switch
            {
                SortField.FirstName => s => s.FirstName,
                SortField.LastName => s => s.LastName,
                SortField.Id => s => s.Id,
                SortField.DateOfBirth => s => s.DateOfBirth,
                SortField.YearOfStudy => s => s.YearOfStudy,

                _ => throw new ArgumentOutOfRangeException(
                    nameof(field),
                    field,
                    "Unsupported sort field.")
            };

            return descending
                ? students.OrderByDescending(key)
                : students.OrderBy(key);
        }
    }
}
