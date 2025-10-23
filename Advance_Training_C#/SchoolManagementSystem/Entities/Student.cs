using SchoolManagementSystem.DTOs;
using System;
using System.Collections.Generic;

namespace SchoolManagementSystem.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime EnrollmentDate { get; set; }

        // Navigation properties
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

     

}

 
    
}