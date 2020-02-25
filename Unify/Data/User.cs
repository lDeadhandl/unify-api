using System.Collections.Generic;
using Google.Cloud.Firestore;

namespace Unify.Data
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string Id { get; set; }      // By convention, a property named Id or <type name>Id will be configured as the key of an entity.

        [FirestoreProperty]
        public string DisplayName { get; set; }

        [FirestoreProperty]
        public string Email { get; set; }
        /// <summary>
        /// Subcription level (premium, free or open)
        /// </summary>
        [FirestoreProperty]
        public string Product { get; set; }
        /// <summary>
        /// The resource identifier that you can enter, for example, in the Spotify Desktop client’s search box to locate the user
        /// </summary>
        [FirestoreProperty]
        public string Uri { get; set; }

        /// <summary>
        /// Used to get a new auth token if the current one is expired
        /// </summary>
        [FirestoreProperty]
        public string SpotifyRefreshToken { get; set; }

        [FirestoreProperty]
        public string SpotifyAccessToken { get; set; }

        [FirestoreProperty]
        public List<Party> Parties { get; set; }
    }
}
