using HealthBook.Model;
using HealthBook.Repository.UserRepo;
using HealthBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<BaseResponseModel> Register(User user)
        {
            try
            {
                return await _userRepository.RegisterAsync(user);
            }
            catch (Exception ex)
            {
                return StatusBuilder.ResponseExceptionStatus(ex);
            }
        }

        [HttpPost("login")]
        public async Task<BaseResponseModel> Login([FromBody] Login login)
        {
            try
            {

                var user = await _userRepository.LoginAsync(login);
                if (user != null)
                {

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var claims = new[] {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                            };
                    var tokeOptions = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidIssuer"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signinCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    return StatusBuilder.ResponseSuccessStatus(new { Token = tokenString });
                }

                return StatusBuilder.ResponseFailStatus(StringConstant.LoginCredentailWrong);
            }
            catch (Exception ex)
            {
                return StatusBuilder.ResponseExceptionStatus(ex);
            }
        }

        [HttpGet("professionals")]
        [Authorize]
        public async Task<BaseResponseModel> GetHealthProfessionals()
        {
            try
            {
                return await _userRepository.GetProfessionalAsync();
            }
            catch (Exception ex)
            {
                return StatusBuilder.ResponseExceptionStatus(ex);
            }
        }
    }
}
