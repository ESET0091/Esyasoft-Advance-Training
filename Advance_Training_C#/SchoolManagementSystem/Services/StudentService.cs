using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.DTOs;
using SchoolManagementSystem.Entities;

namespace SchoolManagementSystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchoolDbContext _context;

        public StudentService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentDto>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Select(s => new StudentDto
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    DateOfBirth = s.DateOfBirth,
                    EnrollmentDate = s.EnrollmentDate
                })
                .ToListAsync();
        }

        public async Task<StudentDto?> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null) return null;

            return new StudentDto
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                EnrollmentDate = student.EnrollmentDate
            };
        }

        public async Task<Student> CreateStudentAsync(CreateStudentDto studentDto)
        {
            var student = new Student
            {
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Email = studentDto.Email,
                DateOfBirth = studentDto.DateOfBirth,
                EnrollmentDate = DateTime.UtcNow
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> UpdateStudentAsync(int id, UpdateStudentDto studentDto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            student.FirstName = studentDto.FirstName;
            student.LastName = studentDto.LastName;
            student.Email = studentDto.Email;
            student.DateOfBirth = studentDto.DateOfBirth;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}