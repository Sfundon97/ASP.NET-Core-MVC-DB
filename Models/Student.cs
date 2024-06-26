﻿// Programmer name : S Nondwatyu
// Student nr : 220036624
// Assignment nr : GA1
// Purpose : The purpose of the Student class is to define the model representing student data
// in an ASP.NET Core web application. It represents a student entity with properties such as StudentNumber,
// FirstName, Surname, EnrollmentDate, Photo, and Email.

using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASPNETCore_DB.Models
{
    public class Student
    {
        [Key]
        [Display(Name = "Student Number")]
        [Required(ErrorMessage = "Student Number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "The Student Number may " +
            "only be 10 digits")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Only digits are allowed")]
        public string? StudentNumber { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The First Name may not be" +
            " shorter 2 characters or more than 50 characters")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Only characters are allowed")]
        public string? FirstName { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Surname is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The Surname may not be" +
            " shorter 2 characters or more than 50 characters")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Only characters are allowed")]
        public string? Surname { get; set; }

        [Display(Name = "Enrollment Date")]
        [Required(ErrorMessage = "Enrollment Date is required")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Photo Indicate")]
        public string Photo {  get; set; }

        [Display(Name = "Contact Mail")]
        [Required(ErrorMessage = "Contact Mail")]
        public string? Email { get; set; }

    }//end class
}//end namespace
