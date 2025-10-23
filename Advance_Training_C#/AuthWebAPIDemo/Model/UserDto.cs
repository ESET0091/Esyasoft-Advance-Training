using AuthWebAPIDemo.Controllers;
using AuthWebAPIDemo.Entities;
using AuthWebAPIDemo.Services;
using Microsoft.Extensions.Configuration;

namespace AuthWebAPIDemo.Model
{
    public class UserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}



