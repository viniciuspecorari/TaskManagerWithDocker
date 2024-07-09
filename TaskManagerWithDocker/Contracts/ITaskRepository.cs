using TaskManagerWithDocker.Core.Dto.Task;
using TaskManagerWithDocker.Models.Entities;

namespace TaskManagerWithDocker.Contracts
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskManager>> GetAllTasks();
        Task<TaskManager> GetTaskById(Guid id);
        Task<TaskManager> CreateTask(TaskCreateDto task);
        Task<bool> UpdateTask(Guid id, TaskUpdateDto task);
        Task<bool> DeleteTask(Guid id);
    }
}
