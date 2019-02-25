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
        private UnifyContext _unifyContext;
        private SpotifyService _spotifyService;

        public ValuesController(
            UnifyContext unifyContext,
            SpotifyService spotifyService)
        {
            _unifyContext = unifyContext;
            _spotifyService = spotifyService;
        }

        // GET api/values
        [HttpGet("{userId}/prediction")]
        public async Task<ActionResult<string[]>> GetPredictions(string userId, string guestId, int x, string tracks)
        {
            var user = await _unifyContext.User.FindAsync(userId);
            var guest = await _unifyContext.User.FindAsync(guestId);

            // Set the authorization
            var userService = await _spotifyService.GetUserService(user.SpotifyAccessToken);// ADD: error handling for tokens
            var userTracks = await userService.GetUserTracks(x); 

            var guestService = await _spotifyService.GetUserService(guest.SpotifyAccessToken);
            var guestTracks = await guestService.GetUserTracks(x);

            userService.Comparator(userTracks, guestTracks);

            var tracksTemp = new List<string>();
            tracksTemp.Add(tracks);

            await userService.GetAudioFeatures(userService.TargetListIds, 0);
            await userService.GetAudioFeatures(tracksTemp, 1);


            var answer = userService.DecisionTree();

            return Ok(answer);
        }

    }
}
