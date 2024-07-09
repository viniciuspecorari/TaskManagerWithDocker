using Microsoft.AspNetCore.Identity;

namespace TaskManagerWithDocker.Core.Entities
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
