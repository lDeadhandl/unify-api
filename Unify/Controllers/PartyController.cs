using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unify.Data;

namespace Unify.Controllers
{
    [ApiController]
    [Route("api/party")]
    public class PartyController : ControllerBase
    {
        private UnifyContext _unifyContext;

        public PartyController(UnifyContext unifyContext)
        {
            _unifyContext = unifyContext;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<Party>>> GetUserParties(string userId)
        {
            if (!await UserExists(userId))
                return BadRequest($"User with id {userId} does not exist");

            var parties = await _unifyContext.Party.Where(x => x.UserId == userId).ToListAsync();

            return Ok(parties);
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> CreateParty(string userId, [FromQuery]string name)
        {
            if (!await UserExists(userId))
                return BadRequest($"User with id {userId} does not exist");

            await _unifyContext.Party.AddAsync(new Party()
            {
                UserId = userId,
                Name = name,
            });

            return Ok();
        }


        // HELPERS

        private Task<bool> UserExists(string userId)
        {
            return _unifyContext.User.AnyAsync(x => x.Id == userId);
        }
    }
}
