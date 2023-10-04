
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);  // return with Ok status code
        }

        [HttpGet("{id}")]  // Notice the change here
        public async Task<ActionResult<User>> GetUser(int id)  // Capture the id parameter
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);  // Filter by id
            if (user == null)
            {
                return NotFound();  // Return 404 if the user is not found
            }
            return Ok(user);
        }
    }
}
