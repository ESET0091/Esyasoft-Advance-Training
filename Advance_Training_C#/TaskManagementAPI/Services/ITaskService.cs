using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync();
        Task<TaskResponseDto?> GetTaskByIdAsync(int id);
        Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto createTaskDto);
        Task<TaskResponseDto?> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto);
        Task<bool> DeleteTaskAsync(int id);
    }
}