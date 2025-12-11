using System;
using Microsoft.AspNetCore.Mvc;
using Backend.Logic;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseHandler _dbHandler = new DatabaseHandler();

        [HttpPost("login")]
        public ActionResult<User> Login([FromBody] LoginRequest request)
        {
            var user = _dbHandler.GetUserByEmail(request.Email);
            if (user != null && user.PasswordHash == request.Password)
            {
                return Ok(user);
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public ActionResult CreateUser([FromBody] RegisterRequest request)
        {
            // enkel kontroll: admin måste ange korrekt hemlig kod från miljövariabel (ADMIN_SECRET)
            var adminSecretCode = Environment.GetEnvironmentVariable("ADMIN_SECRET");

            if (request.IsAdmin)
            {
                if (string.IsNullOrEmpty(adminSecretCode))
                {
                    // Konfigurationsfel på servern – ADMIN_SECRET saknas
                    return StatusCode(500, "Admin-hemlighet saknas i server-konfigurationen.");
                }

                if (!string.Equals(request.AdminSecret, adminSecretCode))
                {
                    return BadRequest("Fel hemlig admin-kod.");
                }
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = request.Password,
                IsAdmin = request.IsAdmin
            };
            _dbHandler.CreateUser(user);
            return Ok();
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string AdminSecret { get; set; }
    }
}