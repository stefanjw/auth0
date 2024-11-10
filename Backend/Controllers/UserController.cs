using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Models;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly YourDbContext _context;

        public UserController(YourDbContext context)
        {
            _context = context;
        }

        [HttpPost("save-profile")]
        [Authorize] // Requires a valid Bearer token
        public async Task<IActionResult> SaveUserProfile([FromBody] User userProfile)
        {
            // Check if the user already exists in the database
            var existingUser = await _context.Users.FindAsync(userProfile.UserId);

            if (existingUser == null)
            {
                // Add new user
                _context.Users.Add(new User
                {
                    UserId = userProfile.UserId,
                    Email = userProfile.Email,
                    Name = userProfile.Name
                });
            }
            else
            {
                // Update existing user's information
                existingUser.Email = userProfile.Email;
                existingUser.Name = userProfile.Name;
                _context.Users.Update(existingUser);
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "User profile saved or updated successfully" });
        }
    }
}

