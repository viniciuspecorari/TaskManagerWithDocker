using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerWithDocker.Contracts;
using TaskManagerWithDocker.Core.Dto.Task;
using TaskManagerWithDocker.Models.Entities;

namespace TaskManagerWithDocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController(ITaskRepository taskRepository) : ControllerBase
    {
        private readonly ITaskRepository _taskRepository = taskRepository;


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TaskManager>>> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAllTasks();

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TaskManager>> GetTaskById(Guid id)
        {
            return await _taskRepository.GetTaskById(id);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TaskManager>> CreateTask(TaskCreateDto task)
        {
            var newTask = await _taskRepository.CreateTask(task);
            return Ok(newTask);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> UpdateTask(Guid id, TaskUpdateDto task)
        {
            await _taskRepository.UpdateTask(id, task);
            return Ok();
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            await _taskRepository.DeleteTask(id);
            return Ok();
        }
    }
}
