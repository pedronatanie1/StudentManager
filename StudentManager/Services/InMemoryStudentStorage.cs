using StudentManager.Models;

namespace StudentManager.Services
{
    /*
    * This class provides an in-memory implementation of IStudentStorage.
    * Students are stored only while the application is running and
    * will be lost when the application closes.
    */
    internal class InMemoryStudentStorage : IStudentStorage
    {
        // Stores students using their unique ID as the key.
        private readonly Dictionary<int, Student> _studentsStorage = new(); 

        public bool Add(Student student) => _studentsStorage.TryAdd(student.Id, student);
        public Student? GetById(int id) => _studentsStorage.GetValueOrDefault(id); 
        public IEnumerable<Student> GetStudents() => _studentsStorage.Values; 
        public bool Remove(int id) => _studentsStorage.Remove(id);
    }
}
