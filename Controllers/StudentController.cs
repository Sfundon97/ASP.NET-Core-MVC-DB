// Programmer name : S Nondwatyu
// Student nr : 220036624
// Assignment nr : GA1
// Purpose : The purpose of this StudentController is to provide basic functionality
// for rendering views and CRUD operations.


using ASPNETCore_DB.Interfaces;
using ASPNETCore_DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCore_DB.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class StudentController : Controller
    {
        private readonly IStudent _studentRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StudentController(IStudent studentRepo, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            // Name:   StudentController
            // Method Parameters :
            //   IStudent studentRepo
            //     - Interface for student repository
            //   IHttpContextAccessor httpContextAccessor
            //     - Provides access to the HttpContext
            //   IWebHostEnvironment webHostEnvironment
            //     - Provides information about the web hosting environment
            try
            {
                _studentRepo = studentRepo;
                _httpContextAccessor = httpContextAccessor;
                _webHostEnvironment = webHostEnvironment;
            }
            catch (Exception ex)
            {
                throw new Exception("Constructor not initialised - IStudent studentRepo");
            }

          
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            // Name : IActionResult Index
            // Purpose : Renders the index view for student records
            // Re-use : None
            // Input Parameters : string sortOrder, string currentFilter, string searchString, int? pageNumber
            //   - Sorting parameters, filter parameters, search string, and page number for pagination
            // Output Type : IActionResult
            //   - Returns the view result for the index view
            pageNumber = pageNumber ?? 1;
            int pageSize = 3;

            ViewData["CurrentSort"] = sortOrder;
            ViewData["StudentNumberSortParm"] = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else 
            { 
                searchString = currentFilter; 
            }

            ViewData["CurrentFilter"] = searchString;

            ViewResult viewResult =  View();

            try
            {
                viewResult = View(PaginatedList<Student>.Create(_studentRepo.GetStudents(searchString, sortOrder).AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            catch (Exception ex) 
            {
                throw new Exception("No student records detected");
            }
                        
            return viewResult;
        }//end method
        public IActionResult Details(string id)
        {
            // Name    : IActionResult Details
            // Purpose : Renders the details view for a specific student
            // Re-use : None
            // Method Parameters : string id
            //   - ID of the student to display details for
            // Output Type : IActionResult
            //   - Returns the view result for the details view
            if (string.IsNullOrEmpty(id))
            {
                var student = _studentRepo.ByEmail(this.User.Identity.Name.ToString());
                if (student != null)
                {
                    return View(student);
                }
            }
            else
            {
                var student = _studentRepo.Details(id);

                    return View(student);
                
            }

            // If the student is not found or if id is null, redirect to the "NotEnrolled" view
            return View("NotEnrolled");
        }//end method


        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult Create()
        {
            //Name : IActionResult Create
            // Purpose : Renders the create view for adding a new student into the database
            // Re-use : None
            // Method Parameters : None
            // Output Type : IActionResult
            //   - Returns the view result for the create view
            var studentExist = _studentRepo.ByEmail(this.User.Identity.Name.ToString());    

             if (studentExist != null)
            {
                return RedirectToAction("Details", "Student", studentExist.StudentNumber);  
            }
            else
            {
            Student student = new Student();
            string fileName = "Default.png";
            student.Photo = fileName;
            return View(student);
            }
             

        }//end method

        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            //Name : IActionResult Create
            // Purpose : Handles form submission to create a new student
            // Re-use : None
            // Method Parameters : Student student
            //   - Student object containing information for the new student
            // Output Type : IActionResult
            //   - Returns the view result after processing the form submission
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string upload = webRootPath + WebConstants.ImagePath;
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);

            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }

            student.Photo = fileName + extension;

            try
            {
                if(ModelState.IsValid)
                {
                    _studentRepo.Create(student);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Student Record not Saved!");
            }

            
            var studentExist = _studentRepo.ByEmail(this.User.Identity.Name.ToString());

            if(studentExist != null)
            {
                return RedirectToAction("Details", "Student", new { id = studentExist.StudentNumber });
            }
            else
            {
                return RedirectToAction("Create");
            }
            

            
        }//end method
        
        
        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            //Name : IActionResult Edit
            // Purpose : Renders the edit view for modifying a student's details
            // Re-use : None
            // Method Parameters : string id
            //   - ID of the student to edit
            // Output Type : IActionResult
            //   - Returns the view result for the edit view
            ViewResult viewDetail = View();
            try
            {
                viewDetail = View(_studentRepo.Details(id));
            }
            catch (Exception ex)
            {
                throw new Exception("Student detail not found");
            }
            return viewDetail;
        }//end method

        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Student student, string photoName)
        {
            //Name : IActionResult Edit
            // Purpose : Handles form submission to modify a student's details
            // Re-use : None
            // Method Parameters : Student student, string photoName
            //   - Student object containing modified details, and the name of the photo
            // Output Type : IActionResult
            //   - Returns the view result after processing the form submission

            if (HttpContext.Request.Form.Files.Count > 0)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                string upload = webRootPath + WebConstants.ImagePath;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);

                if (!string.IsNullOrEmpty(photoName))
                {
                    var oldFile = Path.Combine(upload, photoName);

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                }


                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension),
                    FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                student.Photo = fileName + extension;
            }
            else
            {
                student.Photo = photoName;
            }
            try
            {
                _studentRepo.Edit(student);
            }
            catch (Exception ex)
            {
                throw new Exception("Student detail could not be edited");
            }
            
            return RedirectToAction("Details");
        }//end method

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(string id)
        {
            //Name : IActionResult Delete
            // Purpose : Renders the delete view for removing a student
            // Re-use : None
            // Method Parameters : string id
            //   - ID of the student to delete
            // Output Type : IActionResult
            //   - Returns the view result for the delete view
            ViewResult viewDetail = View();
            try
            {
                viewDetail = View(_studentRepo.Details(id));
            }
            catch (Exception ex)
            {
                throw new Exception("Student detail not found");
            }
            return viewDetail;
        }//end method

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([Bind("StudentNumber, FirstName, Surname, EnrollmentDate")] Student student)
        {
            //Name : IActionResult Delete
            // Purpose : Handles form submission to remove a student from the database
            // Re-use : None
            // Method Parameters : Student student
            //   - Student object containing details of the student to delete
            // Output Type : IActionResult
            //   - Returns the view result after processing the form submission
            try
            {
                _studentRepo.Delete(student);
            }
            catch (Exception ex) 
            {
                throw new Exception("Student could not be deleted");
            }
            
            return RedirectToAction(nameof(Index));
        }//end method


    }//end controller
}//end namespace
