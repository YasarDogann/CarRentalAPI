using CarRentalApi.Business.Operations.User;
using CarRentalApi.Business.Operations.User.Dtos;
using CarRentalApi.WebApi.Jwt;
using CarRentalApi.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addUserDto = new AddUserDto
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                BirthDate = request.BirthDate
            };

            var result = await _userService.AddUser(addUserDto);

            if (result.IsSucceed)
                return Ok();
            else 
                return BadRequest(result.Message);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _userService.LoginUser(new LoginUserDto { Email = request.Email, Password = request.Password });

            if (!result.IsSucceed)
                return BadRequest(result.Message);

            var user = result.Data;

            // bilgiler doğru ise -> JWT
            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>(); // farklı DI yöntemi

            var token = JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType,
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                ExpireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)
            });

            return Ok(new LoginResponse
            {
                Message = "Giriş Başarıyla Gerçekleşti",
                Token = token
            });// şmdi biz burda 200 ile dönmek istemiyoruz sadece tokenı da vermek lazım ve grişi yaıpldı falan mesajı göstermek istiyoruz. Models/LoginResponce
        }

        [HttpGet("me")]
        [Authorize] // tokıen yoksa cevap yok
        public IActionResult GetMyUser()
        {
            return Ok();
        }
    }   
}
