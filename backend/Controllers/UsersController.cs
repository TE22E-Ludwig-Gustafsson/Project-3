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
            if (user != null)
            {
                // Här kan du lägga till lösenordskontroll om du har det
                return Ok(user);
            }
            return Unauthorized();
        }

        [HttpPost]
        public ActionResult CreateUser([FromBody] User user)
        {
            _dbHandler.CreateUser(user);
            return Ok();
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
    }
}