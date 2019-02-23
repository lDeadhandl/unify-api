using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Unify.Service
{
    public class SpotifyUserService
    {
        private JsonSerializerSettings _serializerSettings;
        private HttpClient _client;
        public List<string> Tracks { get; set; }

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

            Tracks = new List<string>();
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

            var profile = JsonConvert.DeserializeObject<UserProfile>(profileJson, _serializerSettings);
            return profile;
        }

        public async Task<List<string>> GetUserTracks(int x)
        {
            //Gets (50 * i) tracks because of paging
            var tracksRequest = Enumerable.Range(0, x).Select(i => _client.GetAsync("https://api.spotify.com/v1/me/tracks?limit=50&offset=" + (i * 50))).ToList();
            var tracksGrab = await Task.WhenAll(tracksRequest);

            foreach (var item in tracksGrab)
            {
                var tracksJson = await item.Content.ReadAsStringAsync();
                var tracks = JsonConvert.DeserializeObject<Paging<SavedTrack>>(tracksJson);
                var trackList = tracks.Items.Select(z => z.Track.Name).ToList();
                Tracks.AddRange(trackList);
            }

            return Tracks;
        }


    }
}
