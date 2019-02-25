using Accord;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Math;
using Accord.Statistics.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;



namespace Unify.Service
{
    public class SpotifyService
    {
        private SpotifyServiceOptions _options;

        private JsonSerializerSettings _serializerSettings;
        private HttpClient _client;
        private SpotifyUserService _userService;

        public SpotifyService(IOptions<SpotifyServiceOptions> spotifyOptions)
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
            _userService = new SpotifyUserService();

            _options = spotifyOptions.Value;
        }

        public async Task<AuthenticationResponse> Authenticate(string token)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", token),
                new KeyValuePair<string, string>("redirect_uri", _options.SpotifyRedirectUri),
                new KeyValuePair<string, string>("client_id", _options.SpotifyClientId),
                new KeyValuePair<string, string>("client_secret", _options.SpotifyClientSecret),
            });

            var tokenRequest = await _client.PostAsync("https://accounts.spotify.com/api/token", content);
            var authorizationJson = await tokenRequest.Content.ReadAsStringAsync();
            var authorization = JsonConvert.DeserializeObject<AuthenticationResponse>(authorizationJson, _serializerSettings);

            return authorization;
        }

        public Task<SpotifyUserService> GetUserService(string accessToken)
        {
            return Task.FromResult(_userService.GetService(accessToken));
        }

    }


}


