using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unify.Data;
using Unify.Service;

namespace Unify.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private UnifyContext _unifyContext;
        private SpotifyService _spotifyService;

        public AccountController(
            UnifyContext unifyContext,
            SpotifyService spotifyService)
        {
            _unifyContext = unifyContext;
            _spotifyService = spotifyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _unifyContext.User.ToListAsync());
        }

        // GET api/values
        [HttpGet("{userId}/tracks")]
        public async Task<ActionResult<List<string>>> GetTracks(string userId, [FromQuery] int x)
        {
            var user = await _unifyContext.User.FindAsync(userId);

            // Set the authorization
            var userService = await _spotifyService.GetUserService(user.SpotifyAccessToken); // ADD: error handling for tokens

            var tracks = await userService.GetUserTracks(x);

            return Ok(tracks);
        }

        [HttpPost]
        public async Task<ActionResult> Authorize(string token)
        {
            // Authenticate the user
            var authentication = await _spotifyService.Authenticate(token);

            // Set the authorization
            var userService = await _spotifyService.GetUserService(authentication.AccessToken);

            var profile = await userService.GetUserProfile();

            // Check if user exists 
            var userExists = await _unifyContext.User.AnyAsync(u => u.Id == profile.Id);

            if (userExists){
            // If user exists, update tokens
                var User = await _unifyContext.User.FindAsync(profile.Id);
                User.SpotifyAccessToken = authentication.AccessToken;
                User.SpotifyRefreshToken = authentication.RefreshToken;
                _unifyContext.SaveChanges();
            }
            else{
            // Add new user with tokens
                _unifyContext.Add(new User
                {
                    DisplayName = profile.DisplayName,
                    Id = profile.Id,
                    SpotifyAccessToken = authentication.AccessToken,
                    SpotifyRefreshToken = authentication.RefreshToken
                });
                _unifyContext.SaveChanges();
            }

            // Return something that is unique to the user and that the backend can use to retrieve their data
            return Ok(profile);
        }

    }
}
