using System;
using System.Collections.Generic;
using System.Text;

namespace Unify.Service
{
    /// <summary>
    /// Application settings for the spotify service
    /// </summary>
    public class SpotifyServiceOptions
    {
        public string SpotifyClientId { get; set; }
        public string SpotifyClientSecret { get; set; }
        public string SpotifyRedirectUri { get; set; }
    }
}
