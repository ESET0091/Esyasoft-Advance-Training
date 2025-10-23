using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.DTOs;
using SchoolManagementSystem.Entities;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public CoursesController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/courses
        [HttpGet]
        public async Task<ActionResult<List<CourseDto>>> GetCourses()
        {
            var courses = await _context.Courses
                .Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    CourseCode = c.CourseCode,
                    CourseName = c.CourseName,
                    Description = c.Description,
                    Credits = c.Credits
                })
                .ToListAsync();

            return Ok(courses);
        }

        // GET: api/courses/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var course = await _context.Courses
                .Where(c => c.CourseId == id)
                .Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    CourseCode = c.CourseCode,
                    CourseName = c.CourseName,
                    Description = c.Description,
                    Credits = c.Credits
                })
                .FirstOrDefaultAsync();

            if (course == null)
                return NotFound($"Course with ID {id} not found");

            return Ok(course);
        }

        // POST: api/courses
        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse(CreateCourseDto courseDto)
        {
            // Check if course code already exists
            var existingCourse = await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseCode == courseDto.CourseCode);

            if (existingCourse != null)
                return BadRequest($"Course with code {courseDto.CourseCode} already exists");

            var course = new Course
            {
                CourseCode = courseDto.CourseCode,
                CourseName = courseDto.CourseName,
                Description = courseDto.Description,
                Credits = courseDto.Credits
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var createdCourse = new CourseDto
            {
                CourseId = course.CourseId,
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                Description = course.Description,
                Credits = course.Credits
            };

            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, createdCourse);
        }

        // PUT: api/courses/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, UpdateCourseDto courseDto)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound($"Course with ID {id} not found");

            // Check if course code is being changed and if it conflicts with existing course
            if (course.CourseCode != courseDto.CourseCode)
            {
                var existingCourse = await _context.Courses
                    .FirstOrDefaultAsync(c => c.CourseCode == courseDto.CourseCode && c.CourseId != id);

                if (existingCourse != null)
                    return BadRequest($"Course with code {courseDto.CourseCode} already exists");
            }

            course.CourseCode = courseDto.CourseCode;
            course.CourseName = courseDto.CourseName;
            course.Description = courseDto.Description;
            course.Credits = courseDto.Credits;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/courses/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound($"Course with ID {id} not found");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/courses/1/enrollments
        [HttpGet("{id}/enrollments")]
        public async Task<ActionResult<List<EnrollmentDto>>> GetCourseEnrollments(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound($"Course with ID {id} not found");

            var enrollments = await _context.Enrollments
                .Where(e => e.CourseId == id)
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