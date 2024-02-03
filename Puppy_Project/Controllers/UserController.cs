using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Puppy_Project.Depandancies;
using Puppy_Project.InputDTOs;
using Puppy_Project.Interfaces;
using Puppy_Project.Models;
using Puppy_Project.Models.Input_OutputDTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Puppy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsers _users;
        private readonly IConfiguration _configuration;
        public UserController(IUsers users,IConfiguration configuration)
        {
            _users=users;
            _configuration=configuration;
        }



        [HttpGet(Name="GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUsers()
        {
            try
            {
                var userslist = _users.ListUsers();
                return userslist != null ? Ok(userslist) : BadRequest("No Users Found");
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UserRegister([FromBody] RegisterDTO user)
        {
            bool isUserAdded = _users.Register(user);
            if (!isUserAdded)
            {
                return BadRequest("User Already Exiset");
            }
            return CreatedAtRoute("GetUsers",new {id=0},user);
        }


        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UserLogin([FromBody] inputUserDTO user)
        {
            var isUser = _users.Login(user);
            if (isUser == null)
            {
                return BadRequest();
            }
            string token = GenerateJwtToken(isUser);
            return Ok(new {id= isUser.Id,token = token});
        }

        private string GenerateJwtToken(outUserDTO user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            // Add additional claims as needed
        };

            var token = new JwtSecurityToken(
                //issuer: _configuration["Jwt:Issuer"],
                //audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationInMinutes"])),
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
