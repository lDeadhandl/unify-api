using System;
using System.Collections.Generic;
using System.Text;

namespace Unify.Service
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        /// <summary>
        /// Requires user-read-email scope
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Subcription level (premium, free or open)
        /// </summary>
        public string Product { get; set; }
        /// <summary>
        /// The resource identifier that you can enter, for example, in the Spotify Desktop client’s search box to locate the user
        /// </summary>
        public string Uri { get; set; }
    }
}
