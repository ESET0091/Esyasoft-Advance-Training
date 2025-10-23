using SchoolManagementSystem.DTOs;
using SchoolManagementSystem.Entities;

namespace SchoolManagementSystem.Services
{
    public interface IStudentService
    {
        Task<List<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto?> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(CreateStudentDto studentDto);
        Task<bool> UpdateStudentAsync(int id, UpdateStudentDto studentDto);
        Task<bool> DeleteStudentAsync(int id);
    }
}