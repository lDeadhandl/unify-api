using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unify.Data
{
    public class User
    {
        public string Id { get; set; }      // By convention, a property named Id or <type name>Id will be configured as the key of an entity.

        public string DisplayName { get; set; }

        public string Email { get; set; }
        /// <summary>
        /// Subcription level (premium, free or open)
        /// </summary>
        public string Product { get; set; }
        /// <summary>
        /// The resource identifier that you can enter, for example, in the Spotify Desktop client’s search box to locate the user
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Used to get a new auth token if the current one is expired
        /// </summary>
        public string SpotifyRefreshToken { get; set; }

        public string SpotifyAccessToken { get; set; }

        public List<Party> Parties { get; set; }
    }
}
