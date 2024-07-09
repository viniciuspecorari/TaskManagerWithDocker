namespace TaskManagerWithDocker.Core.Dto.User
{
    public class ApiUserDto : LoginDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string UserName { get; set; }
    }
}
