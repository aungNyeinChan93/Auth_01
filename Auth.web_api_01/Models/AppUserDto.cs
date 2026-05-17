using Auth.Database_01.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.web_api_01.Models
{
    public class AppUserDto
    {
        public int AppUserId { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public int RoleId { get; set; }

    }

    public class RegisterReqModel
    {
        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public int RoleId { get; set; }

    }

    public class RegisterResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class LoginRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public List<string> Permissions { get; set; }
    }
}
