using System;

namespace SchoolManagementSystem.Entities
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Grade { get; set; } = string.Empty;

        // Navigation properties
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}