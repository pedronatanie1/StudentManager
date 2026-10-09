using StudentManager.Models;

namespace StudentManager.Services
{
    /*
    * Using this interface allows me to swap in-memory storage
    * for JSON or a database later without affecting the rest
    * of the application.
    */
    internal interface IStudentStorage
    {
        bool Add(Student student);              // false if the ID already exists.
        Student? GetById(int id);               // null if not found.
        IEnumerable<Student> GetStudents();     // a sequence I can iterate over.
        bool Remove(int id);                    // false if not found.
    }
}
