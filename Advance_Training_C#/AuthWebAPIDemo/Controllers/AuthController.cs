//using AuthWebAPIDemo.Entities;
//using AuthWebAPIDemo.Model;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace AuthWebAPIDemo.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        public AuthController(IConfiguration configuration)
//        {
//            this.configuration = configuration;
//        }
//        private static User? user = new();
//        private readonly IConfiguration configuration;

//        [HttpPost("register")]
//        public ActionResult<User?>Register(UserDto request)
//        {
//            user.Username = request.Username;
//            user.PasswordHash = new PasswordHasher<User>()
//                .HashPassword(user, request.Password);
//            return Ok(user);

//        }

//        [HttpPost("login")]
//        public ActionResult<string> Login(UserDto request)
//        {
//            if (user.Username != request.Username)
//            {
//                return BadRequest("User not found");
//            }
//            if(new PasswordHasher<User>().VerifyHashedPassword(user,user.PasswordHash, request.Password)== PasswordVerificationResult.Failed)
//            {
//                return BadRequest("Invalid password");
//            }
//            string token = CreateToken(user);
//            return Ok(token);

//        }

//        private string CreateToken(User user)
//        {
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Name, user.Username)
//            };
//            var key = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
//            var tokenDescriptor = new JwtSecurityToken(
//                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
//                audience: configuration.GetValue<string>("AppSettings:Audience"),
//                claims: claims,
//                expires: DateTime.UtcNow.AddDays(1),
//                signingCredentials: creds
//                );
//            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
//        }
//    }
//}





using AuthWebAPIDemo.Entities;
using AuthWebAPIDemo.Model;
using AuthWebAPIDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthWebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthService service;
        public AuthController(IAuthService service)
        {
            this.service = service;
        }    

        [HttpPost("register")]
        public async Task<ActionResult<User?>> Register(UserDto request)
        {
            var user = await service.RegisterAsync(request);
            if (user is null)
                return BadRequest("Username already exist");
            return Ok(user);

        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var token = await service.LoginAsync(request);
            if (token is null)
            {
                return BadRequest("Username/password is wrong");
            }
            return Ok(token);

        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var token = await service.RefreshTokenAsync(request);
            if (token is null)
            {
                return BadRequest("Invalid/expired Token");
            }
            return Ok(token);

        }

        [HttpGet("Auth-end points")]
        [Authorize]
        public ActionResult AuthCheck()
        {
            return Ok();
        }
        [HttpGet("Admin-end points")]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminCheck()
        {
            return Ok();
        }

    }
}
