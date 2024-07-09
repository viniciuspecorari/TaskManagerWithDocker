using Microsoft.EntityFrameworkCore;
using TaskManagerWithDocker.Contracts;
using TaskManagerWithDocker.Core.Dto.Task;
using TaskManagerWithDocker.Data;
using TaskManagerWithDocker.Models.Entities;

namespace TaskManagerWithDocker.Repository
{
    public class TaskRepository(TaskManagerDbContext context) : ITaskRepository
    {
        private readonly TaskManagerDbContext _context = context;

        public async Task<TaskManager> CreateTask(TaskCreateDto task)
        {
            var newTask = new TaskManager
            {
                Title = task.Title,
                Description = task.Description,
                IsComplete = task.IsComplete,
                CreatedAt = DateTime.Now,
            };

            await _context.Tasks.AddAsync(newTask);
            await _context.SaveChangesAsync();

            return newTask;
        }

        public async Task<bool> DeleteTask(Guid id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);

                _context.Tasks.Remove(task);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
                       
        }

        public async Task<IEnumerable<TaskManager>> GetAllTasks()
        {
            var tasks = await _context.Tasks.Select(t => new TaskManager
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsComplete = t.IsComplete,
                CreatedAt = t.CreatedAt,

            }).ToListAsync();

            return tasks;
        }

        public async Task<TaskManager> GetTaskById(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            return task;
        }

        public async Task<bool> UpdateTask(Guid id, TaskUpdateDto task)
        {
            var taskUpdate = await _context.Tasks.FindAsync(id);

            taskUpdate.Title = task.Title ?? taskUpdate.Title;
            taskUpdate.Description = task.Description ?? taskUpdate.Description;
            taskUpdate.IsComplete = task.IsComplete;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
