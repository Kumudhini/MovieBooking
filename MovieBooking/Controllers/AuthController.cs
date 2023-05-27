using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieBooking.Model;
using MovieBooking.Services;
using MovieBooking.Validations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0",Deprecated = true)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _movieService;
        private readonly IConfiguration _config;
        public AuthController(IAuthService movieService, IConfiguration configuration)
        {
            _movieService = movieService;
            _config = configuration;
        }

        [HttpPost("/api/v{version:apiVersion}/moviebooking/register")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> register([FromBody] Register register)
        {
            try
            {
                RegisterValidation validator = new RegisterValidation();
                var result = validator.Validate(register);
                if (result.IsValid)
                {
                    var unique = _movieService.UniqueCheck(register.Email, register.LoginId);
                    if (unique)
                    {
                        var response = await _movieService.Create(register);
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest("Email or LoginId already exists");
                    }
                }
                else
                {
                    var error = validator.Validate(register).Errors.ToList();
                    foreach(var err in error)
                    {
                        return BadRequest(err.ErrorMessage);
                    }
                    return BadRequest(StatusCodes.Status500InternalServerError);
                    
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }            
            

        }

        [HttpPost("/api/v{version:apiVersion}/moviebooking/login")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> login([FromBody] Login login)
        {

            try
            {
                var data = await _movieService.Login(login);
                if (data == null)
                {
                    return BadRequest("LoginId Or Password Invalid");
                }
                var claims = new[]
                    {
                new Claim(ClaimTypes.Name, login.LoginId),
               new Claim(ClaimTypes.Role, data.Role)
            };
                var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("Jwt:Key").Value));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Login Successfully",
                    token = tokenHandler.WriteToken(token),
                });
            }
            catch (Exception)
            {
                return BadRequest("Error Occured while login!");
            }

        }

        [HttpPut("/api/v{version:apiVersion}/moviebooking/forget")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> forgotPassword([FromBody]Forgot forget)
        {
            try
            {
                var response = await _movieService.ForgotPassword(forget);
                return this.Ok(response);

            }
            catch (Exception)
            {
                return BadRequest("Error Occured while updating password!");
            }
           
        }
    }
}
