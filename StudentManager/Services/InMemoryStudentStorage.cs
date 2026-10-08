using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManager.Models
{
    /*
    * This class provides an in-memory implementation of IStudentStorage.
    * Students are stored only while the application is running and
    * will be lost when the application closes.
    */
    internal class InMemoryStudentStorage : IStudentStorage
    {
        // Stores students using their unique ID as the key.
        private readonly Dictionary<int, Student> studentsStorage = new(); 

        public bool Add(Student student) => studentsStorage.TryAdd(student.Id, student);
        public Student? GetById(int id) => studentsStorage.GetValueOrDefault(id); 
        public IEnumerable<Student> GetStudents() => studentsStorage.Values; 
        public bool Remove(int id) => studentsStorage.Remove(id);
    }
}
