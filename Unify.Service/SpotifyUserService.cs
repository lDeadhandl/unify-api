using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Unify.Service
{
    public class SpotifyUserService
    {
        private JsonSerializerSettings _serializerSettings;
        private HttpClient _client;

        public SpotifyUserService()
        {
            // Set the serializer settings to the snake case which is what the spotify responses are formatted as
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            _client = new HttpClient();
        }

        public SpotifyUserService GetService(string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return this;
        }

        public async Task<UserProfile> GetUserProfile()
        {
            var profileRequest = await _client.GetAsync("https://api.spotify.com/v1/me");
            var profileJson = await profileRequest.Content.ReadAsStringAsync();

            var profile = JsonConvert.DeserializeObject<UserProfile>(profileJson);
            return profile;
        }
    }
}
