using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.DTOs;
using SchoolManagementSystem.Entities;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public EnrollmentsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/enrollments
        [HttpGet]
        public async Task<ActionResult<List<EnrollmentDto>>> GetEnrollments()
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Select(e => new EnrollmentDto
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    EnrollmentDate = e.EnrollmentDate,
                    Grade = e.Grade,
                    StudentName = $"{e.Student.FirstName} {e.Student.LastName}",
                    CourseName = e.Course.CourseName,
                    CourseCode = e.Course.CourseCode
                })
                .ToListAsync();

            return Ok(enrollments);
        }

        // GET: api/enrollments/1
        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentDto>> GetEnrollment(int id)
        {
            var enrollment = await _context.Enrollments
                .Where(e => e.EnrollmentId == id)
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Select(e => new EnrollmentDto
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    EnrollmentDate = e.EnrollmentDate,
                    Grade = e.Grade,
                    StudentName = $"{e.Student.FirstName} {e.Student.LastName}",
                    CourseName = e.Course.CourseName,
                    CourseCode = e.Course.CourseCode
                })
                .FirstOrDefaultAsync();

            if (enrollment == null)
                return NotFound($"Enrollment with ID {id} not found");

            return Ok(enrollment);
        }

        // POST: api/enrollments
        [HttpPost]
        public async Task<ActionResult<EnrollmentDto>> CreateEnrollment(CreateEnrollmentDto enrollmentDto)
        {
            // Check if student exists
            var student = await _context.Students.FindAsync(enrollmentDto.StudentId);
            if (student == null)
                return BadRequest("Student not found");

            // Check if course exists
            var course = await _context.Courses.FindAsync(enrollmentDto.CourseId);
            if (course == null)
                return BadRequest("Course not found");

            // Check if enrollment already exists
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == enrollmentDto.StudentId && e.CourseId == enrollmentDto.CourseId);

            if (existingEnrollment != null)
                return BadRequest("Student is already enrolled in this course");

            var enrollment = new Enrollment
            {
                StudentId = enrollmentDto.StudentId,
                CourseId = enrollmentDto.CourseId,
                EnrollmentDate = DateTime.UtcNow,
                Grade = enrollmentDto.Grade
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            var createdEnrollment = new EnrollmentDto
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                EnrollmentDate = enrollment.EnrollmentDate,
                Grade = enrollment.Grade,
                StudentName = $"{student.FirstName} {student.LastName}",
                CourseName = course.CourseName,
                CourseCode = course.CourseCode
            };

            return CreatedAtAction(nameof(GetEnrollment), new { id = enrollment.EnrollmentId }, createdEnrollment);
        }

        // PUT: api/enrollments/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnrollment(int id, UpdateEnrollmentDto enrollmentDto)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
                return NotFound($"Enrollment with ID {id} not found");

            enrollment.Grade = enrollmentDto.Grade;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/enrollments/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
                return NotFound($"Enrollment with ID {id} not found");

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}