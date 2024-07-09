using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerWithDocker.Contracts;
using TaskManagerWithDocker.Core.Dto.User;

namespace TaskManagerWithDocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUserController(IAuthUserRepository authUserRepository, IConfiguration configuration) : ControllerBase
    {
        private readonly IAuthUserRepository _authUserRepository = authUserRepository;
        private readonly IConfiguration _configuration  = configuration;

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
        {
            var errors = await _authUserRepository.Register(apiUserDto);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return Created();
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<JwtTokenDto>> Login([FromBody] LoginDto loginDto)
        {
            var authResponse = await _authUserRepository.Login(loginDto);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            //SetAuthCookies(authResponse.Token, authResponse.RefreshToken);

            return Ok(authResponse);
        }

        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<JwtTokenDto>> RefreshToken()
        {
            if (!this.HttpContext.Request.Cookies.TryGetValue("auth", out var authToken))
            {
                return Unauthorized();
            }

            if (!this.HttpContext.Request.Cookies.TryGetValue("refresh-token", out var refreshToken))
            {
                return Unauthorized();
            }

            var authResponse = await _authUserRepository.VerifyRefreshToken(authToken, refreshToken);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            //SetAuthCookies(authResponse.accessToken, authResponse.refreshToken);

            return Ok(authResponse);
        }

        private void SetAuthCookies(string authToken, string refreshToken)
        {
            this.HttpContext.Response.Cookies.Append("auth", authToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                Path = "/",
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None,
            });

            this.HttpContext.Response.Cookies.Append("refresh-token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(7),
                Path = "/",
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None,

            });
        }
    }
}
