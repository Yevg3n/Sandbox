using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        // TODO: Change it later to search by username instead of id.
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            // TODO: Handle Null User More Explicitly
            return await _context.Users.FindAsync(id);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }
    }
}
