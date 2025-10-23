namespace TaskManagementAPI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public bool IsCompleted { get; set; } = false;
    }

    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
}
