using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.DTOs;
using SchoolManagementSystem.Entities;
using SchoolManagementSystem.Services;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Require authentication for all endpoints
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly SchoolDbContext _context;

        public StudentsController(IStudentService studentService, SchoolDbContext context)
        {
            _studentService = studentService;
            _context = context;
        }

        // GET: api/students
        [HttpGet]
        [AllowAnonymous] // Allow access without authentication
        public async Task<ActionResult<List<StudentDto>>> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        // GET: api/students/1
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound($"Student with ID {id} not found");

            return Ok(student);
        }

        // POST: api/students
        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]  // Only Admin and Teacher can create student
        public async Task<ActionResult<StudentDto>> CreateStudent(CreateStudentDto studentDto)
        {
            try
            {
                // Check if email already exists
                var existingStudent = await _context.Students
                    .FirstOrDefaultAsync(s => s.Email == studentDto.Email);

                if (existingStudent != null)
                    return BadRequest("A student with this email already exists");

                var student = await _studentService.CreateStudentAsync(studentDto);

                var result = new StudentDto
                {
                    StudentId = student.StudentId,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    DateOfBirth = student.DateOfBirth,
                    EnrollmentDate = student.EnrollmentDate
                };

                return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating student: {ex.Message}");
            }
        }

        // PUT: api/students/1
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")] // Only Admin and Teacher can update stude
        public async Task<IActionResult> UpdateStudent(int id, UpdateStudentDto studentDto)
        {
            var result = await _studentService.UpdateStudentAsync(id, studentDto);
            if (!result)
                return NotFound($"Student with ID {id} not found");

            return NoContent();
        }

        // DELETE: api/students/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admin can delete students
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            if (!result)
                return NotFound($"Student with ID {id} not found");

            return NoContent();
        }

        // GET: api/students/1/enrollments
        [HttpGet("{id}/enrollments")]
        public async Task<ActionResult<List<EnrollmentDto>>> GetStudentEnrollments(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound($"Student with ID {id} not found");

            var enrollments = await _context.Enrollments
                .Where(e => e.StudentId == id)
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
    }
}