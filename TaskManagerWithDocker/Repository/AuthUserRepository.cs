using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagerWithDocker.Contracts;
using TaskManagerWithDocker.Core.Dto.User;
using TaskManagerWithDocker.Core.Entities;

namespace TaskManagerWithDocker.Repository
{
    public class AuthUserRepository(UserManager<ApiUser> userManager, IConfiguration configuration) : IAuthUserRepository
    {        
        private readonly UserManager<ApiUser> _userManager = userManager;        
        private readonly IConfiguration _configuration = configuration;
        private ApiUser _user;

        private const string _loginProvider = "TaskAPI";
        private readonly string _refreshToken = "RefreshToken";


        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            var user = new ApiUser
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.email,
                UserName = userDto.email,
            };

            var result = await _userManager.CreateAsync(user, userDto.password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            return result.Errors;
        }

        public async Task<JwtTokenDto> Login(LoginDto login)
        {
            _user = await _userManager.FindByEmailAsync(login.email);
            var isValidUser = await _userManager.CheckPasswordAsync(_user, login.password);

            if (_user == null || isValidUser == false)
            {
                return null;
            }

            var token = await GenerateToken();

            return new JwtTokenDto
            {
                tokenType = "Bearer",
                accessToken = token,
                expiresIn = Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"]),
                refreshToken = await CreateRefreshToken(),
            };
        }


        public async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email ?? ""),
            }
            .Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);

            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);

            await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);

            return newRefreshToken;
        }

        public async Task<JwtTokenDto?> VerifyRefreshToken(string authToken, string refreshToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(authToken);

            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
            var userId = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Sub)?.Value;

            _user = await _userManager.FindByEmailAsync(username);

            if (_user == null || _user.Id != userId)
            {
                return null;
            }

            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, refreshToken);

            string token;
            if (isValidRefreshToken)
            {
                token = await GenerateToken();
                return new JwtTokenDto
                {
                    tokenType = "Bearer",
                    accessToken = token,
                    expiresIn = Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"]),
                    refreshToken = await CreateRefreshToken(),
                };
            }

            await _userManager.UpdateSecurityStampAsync(_user);
            return null;
        }
    }
}
