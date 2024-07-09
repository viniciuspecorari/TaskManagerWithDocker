using Microsoft.AspNetCore.Identity;
using TaskManagerWithDocker.Core.Dto.User;

namespace TaskManagerWithDocker.Contracts
{
    public interface IAuthUserRepository
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);
        Task<JwtTokenDto> Login(LoginDto login);
        Task<string> GenerateToken();
        Task<string> CreateRefreshToken();
        Task<JwtTokenDto?> VerifyRefreshToken(string authToken, string refreshToken);
    }
}
