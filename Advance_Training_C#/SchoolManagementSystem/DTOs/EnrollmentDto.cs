using System;

namespace SchoolManagementSystem.DTOs
{
    public class EnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
    }

    public class CreateEnrollmentDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string Grade { get; set; } = string.Empty;
    }

    public class UpdateEnrollmentDto
    {
        public string Grade { get; set; } = string.Empty;
    }
}