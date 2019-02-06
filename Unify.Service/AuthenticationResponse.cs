using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unify.Service
{
    public class AuthenticationResponse
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public string Scope { get; set; }

        public int ExpiresIn { get; set; }

        public string RefreshToken { get; set; }
    }
}
