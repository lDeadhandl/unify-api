using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unify.Data;
using Unify.ViewModel;

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
        public async Task<ActionResult<PartyVM>> GetUserParty(string userId)
        {
            if (!await UserExists(userId))
                return BadRequest($"User with id {userId} does not exist");

            var user = await _unifyContext.User
                .Include(u => u.Parties)
                .FirstAsync(u => u.Id == userId);

            var party = user.Parties.FirstOrDefault();
            if (party == null)
                return Ok(null);

            // Map to the view model PartyVM
            var partyVm = new PartyVM
            {
                Id = party.Id,
                Name = party.Name
            };

            return Ok(partyVm);
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

            await _unifyContext.SaveChangesAsync();

            return Ok();
        }


        // HELPERS

        private Task<bool> UserExists(string userId)
        {
            return _unifyContext.User.AnyAsync(x => x.Id == userId);
        }
    }
}
