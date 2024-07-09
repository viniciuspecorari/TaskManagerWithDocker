namespace TaskManagerWithDocker.Models.Entities
{
    public class TaskManager
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
