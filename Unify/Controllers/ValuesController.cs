using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Unify.Data;
using Unify.Service;

namespace Unify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public SpotifyServiceOptions Options { get; set; }
        private readonly UnifyContext _context;

        //Dependency injection for options from appsettings.json
        public ValuesController(IOptions<SpotifyServiceOptions> optionsAccessor, UnifyContext context)
        {
            Options = optionsAccessor.Value;
            _context = context;

            if (_context.User.Count() == 0)
            {
                // Create a new user if collection is empty,
                // which means you can't delete all users.
                _context.User.Add(new User { DisplayName = "Item1" });
                _context.SaveChanges();
            }
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var User = await _context.User.FindAsync(id);

            if (User == null)
            {
                return NotFound();
            }

            return User;
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var userExists = await _context.User.AnyAsync(u => u.Id == user.Id);
            if (userExists)
                return BadRequest($"User with id {user.Id} already exists.");

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            //change so the id matches the new spot's available id
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
