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
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _movieService;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService movieService, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _movieService = movieService;
            _config = configuration;
            _logger = logger;
        }

        [HttpPost(Constants.RoutingConstant.Register)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> register([FromBody] Register register)
        {
            try
            {
                _logger.LogInformation("For New Register Request received for the Id :{id} ",register._id);
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
                        _logger.LogError("EmailId or LoginId already Present for the request Id : {id}",register._id);
                        return BadRequest(new
                        {
                            StatusCode = Constants.Constant.NotFound,
                            Message = Constants.Constant.EmailLoginNotFound,
                        });
                    }
                }
                else
                {
                    _logger.LogError("Something Went Wrong While process the request Id :{id}",register._id);
                    return BadRequest(StatusCodes.Status500InternalServerError);
                    
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }            
            

        }

        [HttpPost(Constants.RoutingConstant.Login)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> login([FromBody] Login login)
        {

            try
            {
                _logger.LogInformation("Login Request Received for the LoginId : {id}", login.LoginId);
                var data = await _movieService.Login(login);
                if (data == null)
                {
                    _logger.LogError("LoginId Or Password Invalid for the User :{user}",login.LoginId);
                    return BadRequest(new
                    {
                        StatusCode = Constants.Constant.NotFound,
                        Message = Constants.Constant.LoginInvalid,
                    });
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
                    Expires = DateTime.Now.AddMinutes(30),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new
                {
                    StatusCode = Constants.Constant.OkResponse,
                    Message = Constants.Constant.LoginSuccessfull,
                    token = tokenHandler.WriteToken(token),
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new
                {
                    StatusCode = Constants.Constant.NotFound,
                    Message = Constants.Constant.ErrorOccureLogin,
                });
            }

        }

        [HttpPut(Constants.RoutingConstant.ForgetPassword)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> forgotPassword(string username, ForgetPassword forget)
        {
            try
            {
                _logger.LogInformation("Forget Password Request Received for the UserName :{id}",username);
                var response = await _movieService.ForgotPassword(username,forget);
                return Ok(new
                {
                    StatusCode = 200,
                    Message = response,
                });

            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message);
                return BadRequest(new
                {
                    StatusCode = Constants.Constant.NotFound,
                    Message = Constants.Constant.ErrorOccurForPasswordUpdate,
                }) ;
            }
           
        }
    }
}
