namespace TaskManagerWithDocker.Core.Dto.Task
{
    public record TaskCreateDto(string Title, string Description, bool IsComplete);
}
