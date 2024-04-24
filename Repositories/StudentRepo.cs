// Programmer name: S Nondwatyu
// Student nr: 220036624
// Assignment nr: GA1
// Purpose: Implement the StudentRepo class, which implements the IStudent interface,
// to provide CRUD functionality for interacting with student data in an ASP.NET Core application.
// 

using ASPNETCore_DB.Data;
using ASPNETCore_DB.Interfaces;
using ASPNETCore_DB.Models;


namespace ASPNETCore_DB.Repositories
{
    public class StudentRepo : IStudent
    {
        private readonly SQLiteDBContext _context;

        public StudentRepo(SQLiteDBContext context)
        {
            _context = context;
        }

        public Student ByEmail(string id)
        {
            // Name : Student ByEmail(string id)
            // Purpose : Retrieve a student from the database based on their email address.
            // Method Parameters : string id
            // - The email address of the student to retrieve.
            // Output Type : Student
            // - The student record corresponding to the given email address.

            var student = _context.Students?.FirstOrDefault(x => x.Email == id);
            return student;
        }

        public Student Create(Student student)
        {
            // Name : Student Create(Student student)
            // Purpose : Create a new student record in the database.
            // Method Parameters : Student student
            // - The student record to be created.
            // Output Type : Student
            // - The created student entity.
            _context.Add(student);
            _context.SaveChanges();
            return student;
        }

        public bool Delete(Student student)
        {
            // Name : bool Delete(Student student)
            // Purpose : Delete a student record from the database.
            // Method Parameters : Student student
            // - The student record to be deleted.
            // Output Type : bool
            // - Returns true if the student was successfully deleted, false otherwise.
            _context.Remove(student);
            _context.SaveChanges();
            return IsExist(student.StudentNumber);
        }

        public Student Details(string id)
        {
            // Name : Student Details(string id)
            // Purpose : Retrieve a student from the database based on their student number.
            // Method Parameters : string id
            // - The student number of the student to retrieve.
            // Output Type : Student
            // - The student record corresponding to the given student number.
            var student = _context.Students?.FirstOrDefault(x => x.StudentNumber == id);
            return student;
        }

        public Student Edit(Student student)
        {
            // Name : Student Edit(Student student)
            // Purpose : Update an existing student record in the database.
            // Method Parameters : Student student
            // - The student entity containing updated information.
            // Output Type : Student
            // - The updated student record.
            _context.Update(student);
            _context.SaveChanges();
            return student;
        }

        public IQueryable<Student> GetStudents(string searchString, string sortOrder)
        {
            // Name : IQueryable<Student> GetStudents(string searchString, string sortOrder)
            // Purpose : Retrieve a list of students from the database based on search criteria and sorting order.
            // Method Parameters : string searchString, string sortOrder
            // - searchString: The search string used to filter students by student number.
            // - sortOrder: The sorting order for the retrieved students.
            // Output Type : IQueryable<Student>
            // - A queryable collection of student entities matching the search criteria and sorted accordingly.
            var student = _context.Students
               .ToList();
            if(!String.IsNullOrEmpty(searchString)) 
            {
                student = student.Where(s => s.StudentNumber.Contains(searchString)).ToList();
            }
            switch (sortOrder)
            {
                case "number_desc":
                    student = student.OrderByDescending(s => s.StudentNumber).ToList();
                    break;
                case "name_desc":
                    student = student.OrderByDescending(s => s.Surname).ToList();
                    break;
                case "Date":
                    student = student.OrderBy(s => s.EnrollmentDate).ToList();
                    break;
                case "date_desc":
                    student = student.OrderByDescending(s => s.EnrollmentDate).ToList();
                    break;
                default:
                    student = student.OrderBy(s => s.Surname).ToList();
                    break;
            }

            return student.AsQueryable();
        }

        public bool IsExist(string id)
        {
            // Name : bool IsExist(string id)
            // Purpose : Check if a student with the given student number exists in the database.
            // Method Parameters : string id
            // - The student number to check for existence.
            // Output Type : bool
            // - Returns true if the student exists, false otherwise.
            bool isExist = false;
            Student existStudent = Details(id);
            if (existStudent == null)
            {
                isExist = true;
            }
            return isExist;
        }

    }
}
