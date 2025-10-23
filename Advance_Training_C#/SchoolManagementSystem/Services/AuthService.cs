using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SchoolManagementSystem.Entities;
using SchoolManagementSystem.DTOs;

namespace SchoolManagementSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // 1. Find user by email
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                throw new Exception("Invalid email or password");

            // 2. Check password
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
                throw new Exception("Invalid email or password");

            // 3. Generate JWT token
            var token = await GenerateJwtToken(user);

            return token;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // 1. Check if passwords match
            if (registerDto.Password != registerDto.ConfirmPassword)
                throw new Exception("Passwords do not match");

            // 2. Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists");

            // 3. Check if role exists
            //var roleExists = await _roleManager.RoleExistsAsync(registerDto.Role);
            //if (!roleExists)
            //    throw new Exception($"Role '{registerDto.Role}' does not exist");

            // 4. Create new user
            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };

            // 5. Create user
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            // 6. Assign role to user
            await _userManager.AddToRoleAsync(user, registerDto.Role);

            // 7. Generate JWT token
            var token = await GenerateJwtToken(user);

            return token;
        }

        private async Task<AuthResponseDto> GenerateJwtToken(User user)
        {
            // 1. Create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Roles),           
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // 2. Add roles to claims
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 3. Get secret key from configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? throw new Exception("JWT Key not configured")));

            // 4. Create signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 5. Create token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"])),
                signingCredentials: creds
            );

            // 6. Write token
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDto
            {
                Token = tokenString,
                Expiration = token.ValidTo,
                UserId = user.Id,              
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList()
            };
        }
    }
}