using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost]
        public async Task<ActionResult> Authorize(string token)
        {
            // Authenticate the user
            var authentication = await _spotifyService.Authenticate(token);

            // Set the authorization
            var userService = await _spotifyService.GetUserService(authentication.AccessToken);

            var profile = await userService.GetUserProfile();





            return Ok();
        }
    }
}
