using Auth_system.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Auth_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /*
         * We are using a Controllerbase class to directly handle 
         * requests 
         */
        
        
        public static User user = new User();
        private readonly IConfiguration _configuration;

        // Constructor
        public AuthController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }


        [HttpPost("register")]
        public ActionResult<User> Register(UserDTO request)
        {
            string pwdhash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.UserName = request.UserName;
            user.PasswordHash = pwdhash;

            return Ok(user);
        }


        [HttpPost("login")]
        public ActionResult<User> Login(UserDTO request)
        {
            if (user.UserName != request.UserName)
            {
                return BadRequest("User not found !");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong pass");
            }
            string token = CreateToken(user);
            return Ok(user);
        }


        private string CreateToken(User user)
        {
            /*
             * A method for creating a user token 
             * for his navigation
             */

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value! ));

            var creds = new SigningCredentials(key , SecurityAlgorithms.HmacSha512Signature );
           
            var token = new JwtSecurityToken(
                claims:claims,
                expires : DateTime.Now.AddDays(1) , // will expire in a day
                signingCredentials : creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }
    }
}
