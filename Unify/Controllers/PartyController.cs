//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using System.Threading.Tasks;
//using Unify.Data;
//using Unify.ViewModel;

//namespace Unify.Controllers
//{
//    [ApiController]
//    [Route("api/party")]
//    public class PartyController : ControllerBase
//    {


//        [HttpGet("{userId}")]
//        public async Task<ActionResult<PartyVM>> GetUserParty(string userId)
//        {
//            // Check if user exists
//            if (!await UserExists(userId))
//                return BadRequest($"User with id {userId} does not exist");

//            var user = await _unifyContext.User
//                .Include(u => u.Parties)
//                .FirstAsync(u => u.Id == userId);

//            var party = user.Parties
//                .FirstOrDefault();
//            if (party == null)
//                return Ok(null);

//            var guests = await _unifyContext.Guests
//                .Include(u => u.User)
//                .Where(u => u.PartyId == party.Id)
//                .ToListAsync();

//            // Map to the view model PartyVM
//            var partyVm = new PartyVM
//            {
//                Id = party.Id,
//                Name = party.Name,
//                Guests = guests.Select(u => u.UserId).ToList()
//            };

//            return Ok(partyVm);
//        }

//        [HttpPost("{userId}")]
//        public async Task<ActionResult> CreateParty(string userId, [FromQuery]string name)
//        {



//            // Check if user exists
//            if (!await UserExists(userId))
//                return BadRequest($"User with id {userId} does not exist");

//            var user = await _unifyContext.User
//            .Include(u => u.Parties)
//            .FirstAsync(u => u.Id == userId);

//            var party = user.Parties
//            .FirstOrDefault();

//            var guest = await _unifyContext.Guests
//            .AnyAsync(u => u.UserId == userId);

//            // Check if user is in a party
//            if (party != null || guest)
//                return BadRequest($"User with id {userId} already has a party");

//            // Add new party to Db
//            await _unifyContext.Party.AddAsync(new Party()
//            {
//                UserId = userId,
//                Name = name,
//            });

//            await _unifyContext.SaveChangesAsync();

//            return Ok();
//        }

//        [HttpPost("{userId}/add")]
//        public async Task<ActionResult> AddMember(string userId, string guestId)
//        {
//            // Check if users exist
//            if (!await UserExists(userId) || !await UserExists(guestId))
//                return BadRequest($"User with id {userId} does not exist");

//            var leader = await _unifyContext.User
//            .Include(u => u.Parties)
//            .FirstAsync(u => u.Id == userId);

//            var party = leader.Parties.FirstOrDefault();
//            if (party == null)
//                return Ok(null);

//            var guest = await _unifyContext.Guests
//            .AnyAsync(u => u.UserId == guestId);

//            // Check if user is in a party
//            if (guest)
//                return BadRequest($"User with id {userId} already has a party");

//            // Create guest with partyId matching leaders partyId
//            await _unifyContext.AddAsync(new Guests
//            {
//                UserId = guestId,
//                PartyId = party.Id,
//            });

//            _unifyContext.SaveChanges();

//            return Ok();

//        }

//        [HttpDelete("{userId}")]
//        public async Task<ActionResult> RemoveGuest(string userId, string guestId)
//        {
//            // Check if users exist
//            if (!await UserExists(userId) || !await UserExists(guestId))
//                return BadRequest($"User with id {userId} does not exist");

//            // check if user with guestid is in the party
//            var guest = await _unifyContext.Guests.Where(u => u.UserId == guestId).FirstAsync();

//            _unifyContext.Guests.Remove(guest);
//            await _unifyContext.SaveChangesAsync();

//            return Ok();

//        }

//        [HttpDelete("{userId}/removeparty")]
//        public async Task<ActionResult> RemoveParty(string userId)
//        {
//            // Check if users exist
//            if (!await UserExists(userId))
//                return BadRequest($"User with id {userId} does not exist");

//            var user = await _unifyContext.User
//            .Include(u => u.Parties)
//            .FirstAsync(u => u.Id == userId);

//            var party = user.Parties
//                .FirstOrDefault();
//            if (party == null)
//                return Ok(null);

//            _unifyContext.Party.Remove(party);
//            await _unifyContext.SaveChangesAsync();

//            return Ok();
//        }

//        // HELPERS

//        private Task<bool> UserExists(string userId)
//        {
//            return _unifyContext.User.AnyAsync(x => x.Id == userId);
//        }
//    }
//}
