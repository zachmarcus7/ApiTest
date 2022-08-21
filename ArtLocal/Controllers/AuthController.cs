using ArtLocal.Data;
using ArtLocal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ArtLocal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ArtLocalDataContext _dbContext;
        private IConfiguration _config;

        public AuthController(ArtLocalDataContext context, IConfiguration config)
        {
            _dbContext = context;
            _config = config;
        }


        // Authenticate an Admin's login request
        // localhost:port/api/Auth/Admin
        [HttpPost("Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] Admin admin)
        {
            // check if the admin exists in the database
            Admin test = await _dbContext.Admins.FirstOrDefaultAsync(user => user.Username == admin.Username);

            if (test == null)
            {
                return NoContent();
            }

            // test if the credentials match
            if (admin.Username == test.Username && admin.Password == test.Password)
            {
                // if there's a match, then generate a new JWT token to send back
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                // here, claims support data about the admin
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, admin.Username),
                    new Claim(ClaimTypes.Role, "admin")
                };

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: credentials);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }

            return NoContent();
        }
        

        // Authenticate a Customer's login request
        // localhost:port/api/Auth/Customer
        [HttpPost("Customer")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] Customer customer)
        {
            // check if the customer exists in the database
            Customer test = await _dbContext.Customers.FirstOrDefaultAsync(user => user.Username == customer.Username);

            if (test == null)
            {
                return NoContent();
            }

            // test if the credentials match
            if (customer.Username == test.Username && customer.Password == test.Password)
            {
                // if there's a match, then generate a new JWT token to send back
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                // here, claims support data about the admin
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, customer.Username),
                    new Claim(ClaimTypes.Role, "customer"),
                };

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: credentials);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }

            return NoContent();
        }

    }
}
