using Google.Cloud.Firestore;
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
        private SpotifyService _spotifyService;

        public AccountController(
            SpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }



        // GET api/values
        [HttpGet("{userId}/tracks")]
        public async Task<ActionResult<List<string>>> GetTracks(string userId, [FromQuery] int x)
        {
            var db = FirestoreDb.Create("unify-database");

            var snapshot = await db.Collection("Users").Document(userId).GetSnapshotAsync();

            var SAT = snapshot.ToDictionary()["SpotifyAccessToken"];

            //// Set the authorization
            var userService = await _spotifyService.GetUserService(SAT.ToString()); // ADD: error handling for tokens

            var tracks = await userService.GetUserTracks(x);

            return Ok(tracks);
        }

        // POST 
        [HttpPost("add")]
        public async Task<ActionResult> AddUsers(string project)
        {
            var db = FirestoreDb.Create(project);
            DocumentReference docRef = db.Collection("users").Document("al");
            Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "First", "ASDFASDF" },
                { "Last", "WAAAAAAAAAAAA" },
                { "Born", 1815 }
            };
            await docRef.SetAsync(user);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Authorize(string token)
        {
            var db = FirestoreDb.Create("unify-database");

            // Authenticate the user
            var authentication = await _spotifyService.Authenticate(token);

            // Set the authorization
            var userService = await _spotifyService.GetUserService(authentication.AccessToken);

            var profile = await userService.GetUserProfile();

            var docRef = db.Collection("Users").Document($"{profile.Id}");

            var User = new User() {
                DisplayName = profile.DisplayName,
                SpotifyAccessToken = authentication.AccessToken,
                SpotifyRefreshToken = authentication.RefreshToken
            };
            await docRef.SetAsync(User);

            // Return something that is unique to the user and that the backend can use to retrieve their data
            return Ok(profile);
        }

    }
}
