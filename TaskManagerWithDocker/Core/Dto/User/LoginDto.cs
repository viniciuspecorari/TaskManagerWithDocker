using TaskManagerWithDocker.Core.Entities;

namespace TaskManagerWithDocker.Core.Dto.User
{
    public class LoginDto
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
