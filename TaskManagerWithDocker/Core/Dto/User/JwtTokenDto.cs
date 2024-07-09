namespace TaskManagerWithDocker.Core.Dto.User
{
    public class JwtTokenDto
    {
        public string tokenType { get; set; }
        public string accessToken { get; set; }
        public int expiresIn { get; set; }
        public string refreshToken { get; set; }
    }
}
