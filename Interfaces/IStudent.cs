// Programmer name : S Nondwatyu
// Student nr : 220036624
// Assignment nr : GA1
// Purpose : The purpose of this IStudent interface is to define a contract for student-related operations.
// It declares methods for common CRUD (Create, Read, Update, Delete) operations
// and additional functionality such as querying students based on search criteria,
// retrieving student details by ID, and checking for the existence of a student.


using ASPNETCore_DB.Models;

namespace ASPNETCore_DB.Interfaces
{
    public interface IStudent
    {
        IQueryable<Student> GetStudents(string searchString, string sortOrder);
        Student Details(string id);
        Student Create(Student student);
        Student ByEmail(string id);
        Student Edit(Student student);
        bool Delete(Student student);
        bool IsExist(string id);
    }
}
