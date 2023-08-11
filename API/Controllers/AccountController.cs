using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;
        }

        // TODO: Take JSON instead of parameters
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto registerDto)
        {
            // TODO: Ensure that the provided data is valid and meets the required criteria
            // TODO: Check if username already exists in the database
            using var hmac = new HMACSHA512();
            var user = new User
            {
                Username = registerDto.Username,
                // TODO: Consider using the Task.Run pattern to perform the CPU-intensive password hashing asynchronously.
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
