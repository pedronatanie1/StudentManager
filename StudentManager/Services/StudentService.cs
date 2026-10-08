using StudentManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManager.Services
{
    internal class StudentService 
    {
        // Depend on the storage contract rather than a specific storage implementation.
        private readonly IStudentStorage _storage;
        public StudentService (IStudentStorage storage)
        {
            _storage = storage;
        }

        public bool AddStudent(Student student) => _storage.Add(student);
        public Student? FindById(int id) => _storage.GetById(id);
        public IEnumerable<Student> GetAll() => _storage.GetStudents();
        public bool RemoveStudent(int id) => _storage.Remove(id);

        // Update, Search, Filter, Sort, Statistics come later
    }
}
