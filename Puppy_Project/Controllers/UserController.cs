using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IUsersService _users;
        private readonly IConfiguration _configuration;
        public UserController(IUsersService users,IConfiguration configuration)
        {
            _users=users;
            _configuration=configuration;
        }



        [HttpGet(Name="GetUsers")]
        [Authorize(Roles="admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var userslist = await _users.ListUsers();
                return userslist != null ? Ok(userslist) : BadRequest("No Users Found");
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var userslist = await _users.GetUser(id);
                return userslist != null ? Ok(userslist) : BadRequest("No Users Found");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("AddAdmin")]
        [Authorize(Roles="admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewAdmin([FromBody] RegisterDTO user)
        {
            try
            {
                bool isUserAdded = await _users.AddNewAdmin(user);
                if (!isUserAdded)
                {
                    return BadRequest("Already Exiset");
                }
                return CreatedAtRoute("GetUsers", new { id = 0 }, user);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }


        [HttpPut("BlockUser/{id:int}")]
        [Authorize(Roles="admin")]
        public async Task<IActionResult> BlockUser(int id)
        {
            try
            {
                bool isBlocked = await _users.BlockUser(id);
                return isBlocked ? Ok(id) : BadRequest();
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPut("UnBlockUser/{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UnBlockUser(int id)
        {
            try
            {
                bool isBlocked = await _users.UnBlockUser(id);
                return isBlocked ? Ok(id) : BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }



        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UserRegister([FromBody] RegisterDTO user)
        {
            try
            {
                bool isUserAdded = await _users.Register(user);
                return isUserAdded? Ok(user.Email) : BadRequest("User Already Exiset");
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }


        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UserLogin([FromBody] inputUserDTO user)
        {
            try
            {
                var isUser = await _users.Login(user);
                if (isUser == null || isUser.Role == null)
                {
                    return BadRequest();
                }
                string token = GenerateJwtToken(isUser);
                return Ok(new { id = isUser.Id, token = token });
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
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
