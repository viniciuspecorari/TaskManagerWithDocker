namespace TaskManagerWithDocker.Core.Dto.Task
{
    public record TaskUpdateDto(string? Title, string? Description, bool IsComplete);
}
