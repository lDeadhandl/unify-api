using Accord;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Math;
using Accord.Statistics.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
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
        public List<AudioFeatures> audioFeatures { get; set; }
        public List<AudioFeatures> audioFeaturestest { get; set; }
        public List<string> TargetList { get; set; }
        public List<int> TargetValues { get; set; }
        public List<string> TargetListIds { get; set; }

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

            audioFeatures = new List<AudioFeatures>();
            audioFeaturestest = new List<AudioFeatures>();



            // Used in comparator function
            TargetList = new List<string>();
            TargetValues = new List<int>();
            TargetListIds = new List<string>();
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
            var Tracks = new List<string>();
            //Gets (50 * i) tracks because of paging
            var tracksRequest = Enumerable.Range(0, x).Select(i => _client.GetAsync("https://api.spotify.com/v1/me/tracks?limit=50&offset=" + (i * 50))).ToList();
            var tracksGrab = await Task.WhenAll(tracksRequest);

            foreach (var item in tracksGrab)
            {
                var tracksJson = await item.Content.ReadAsStringAsync();
                var tracks = JsonConvert.DeserializeObject<Paging<SavedTrack>>(tracksJson, _serializerSettings);
                var trackList = tracks.Items.Select(z => z.Track.Name).ToList();
                Tracks.AddRange(trackList);
            }

            return Tracks;
        }

        public void Comparator(List<string> Stef, List<string> Chris)
        {

            //Creates temporary combined list of songs to be used later in function
            var FullList = new List<string>();
            FullList.AddRange(Stef);
            FullList.AddRange(Chris);

            //Creates list of unmatched items: Target0
            var Target0 = new List<string>();
            var firstNotSecond = Stef.Except(Chris).ToList();
            var secondNotFirst = Chris.Except(Stef).ToList();
            Target0.AddRange(firstNotSecond);
            Target0.AddRange(secondNotFirst);

            var Target1 = FullList.Except(Target0).ToList();

            //List of Songs that match(1) and do not match(0) with 1's being first in the list 
            TargetList.AddRange(Target1);
            TargetList.AddRange(Target0);

            var partition = Partition<string>(TargetList, 100);

            //Creates comma separated list of Ids to be used in GetFeatures function
            foreach (var item in partition)
            {
                TargetListIds.Add(string.Join(',', item.Select(e => e)));
            }

            //Adds to List of Target values that corresponds with the order of TargetList
            TargetValues.AddRange(Enumerable.Repeat(1, Target1.Count()));
            TargetValues.AddRange(Enumerable.Repeat(0, Target0.Count()));
        }

                //Takes a list and breaks it into a list of smaller lists with a specified size.
        //This was used to break up the target tracklist down into lists of 100 due to the GET audio features API limit
        public static IEnumerable<List<T>> Partition<T>(IList<T> source, Int32 size)
        {
            for (int i = 0; i < Math.Ceiling(source.Count / (Double)size); i++)
                yield return new List<T>(source.Skip(size * i).Take(size));
        }

        // toggle 0 = decision tree, toggle 1 = test
        public async Task GetAudioFeatures(List<string> Ids, int toggle)
        {
            foreach (var item in Ids)
            {
                var l = await _client.GetAsync("https://api.spotify.com/v1/audio-features?ids=" + item);
                var featuresResponse = await l.Content.ReadAsStringAsync();
                var features = JsonConvert.DeserializeObject<AudioFeaturesObject>(featuresResponse, _serializerSettings);

                if (toggle == 0)
                {
                    audioFeatures.AddRange((features.AudioFeatures.Select(z => z)).ToList());
                }
                else
                {

                    audioFeaturestest.AddRange((features.AudioFeatures.Select(z => z)).ToList());
                }

            }
        }

        public string[] DecisionTree()
        {
            // Create new test table
            DataTable datatest = new DataTable("test table");
            datatest.Columns.Add("Name", "Acousticness", "Danceability", "Energy",
                "Instrumentalness", "Liveness", "Loudness", "Speechiness", "Tempo", "Valence", "Target");

            // Create new training table
            DataTable data = new DataTable("Mitchell's Tennis Example");
            data.Columns.Add("Name", "Acousticness", "Danceability", "Energy",
                "Instrumentalness", "Liveness", "Loudness", "Speechiness", "Tempo", "Valence", "Target");

            // Populating both tables
            foreach (var item in TargetList)
            {
                data.Rows.Add(item.ToString());
            }

            for (int i = 0; i < audioFeatures.Count; i++)
            {

                data.Rows[i]["Acousticness"] = audioFeatures[i].Acousticness;
                data.Rows[i]["Danceability"] = audioFeatures[i].Danceability;
                data.Rows[i]["Energy"] = audioFeatures[i].Energy;
                data.Rows[i]["Instrumentalness"] = audioFeatures[i].Instrumentalness;
                data.Rows[i]["Liveness"] = audioFeatures[i].Liveness;
                data.Rows[i]["Loudness"] = audioFeatures[i].Loudness;
                data.Rows[i]["Speechiness"] = audioFeatures[i].Speechiness;
                data.Rows[i]["Tempo"] = audioFeatures[i].Tempo;
                data.Rows[i]["Valence"] = audioFeatures[i].Valence;
            }

            for (int i = 0; i < TargetValues.Count; i++)
            {
                data.Rows[i]["Target"] = TargetValues[i].ToString();
            }

            for (int i = 0; i < audioFeaturestest.Count; i++)
            {
                datatest.Rows.Add("0");
            }

            for (int i = 0; i < audioFeaturestest.Count; i++)
            {

                datatest.Rows[i]["Acousticness"] = audioFeaturestest[i].Acousticness;
                datatest.Rows[i]["Danceability"] = audioFeaturestest[i].Danceability;
                datatest.Rows[i]["Energy"] = audioFeaturestest[i].Energy;
                datatest.Rows[i]["Instrumentalness"] = audioFeaturestest[i].Instrumentalness;
                datatest.Rows[i]["Liveness"] = audioFeaturestest[i].Liveness;
                datatest.Rows[i]["Loudness"] = audioFeaturestest[i].Loudness;
                datatest.Rows[i]["Speechiness"] = audioFeaturestest[i].Speechiness;
                datatest.Rows[i]["Tempo"] = audioFeaturestest[i].Tempo;
                datatest.Rows[i]["Valence"] = audioFeaturestest[i].Valence;
            }

            for (int i = 0; i < audioFeaturestest.Count; i++)
            {
                datatest.Rows[i]["Target"] = TargetValues[i].ToString();
            }

            var codebooktest = new Codification(datatest);

            //// Translate our training data into integer symbols using our codebook:
            DataTable symbols1 = codebooktest.Apply(datatest);
#pragma warning disable CS0618 // Type or member is obsolete
            double[][] test = datatest.ToArray<double>("Acousticness", "Danceability", "Energy", "Instrumentalness", "Liveness", "Loudness", "Speechiness", "Tempo", "Valence");

            var codebook = new Codification(data);

            //// Translate our training data into integer symbols using our codebook:
            DataTable symbols = codebook.Apply(data);
#pragma warning disable CS0618 // Type or member is obsolete
            double[][] inputs = data.ToArray<double>("Acousticness", "Danceability", "Energy", "Instrumentalness", "Liveness", "Loudness", "Speechiness", "Tempo", "Valence");
#pragma warning restore CS0618 // Type or member is obsolete
            int[] outputs = symbols.ToArray<int>("Target");

            // Create a teaching algorithm:
            var teacher = new C45Learning()
            {
                    new DecisionVariable("Acousticness",     DecisionVariableKind.Continuous),
                    new DecisionVariable("Danceability", DecisionVariableKind.Continuous),
                    new DecisionVariable("Energy",    DecisionVariableKind.Continuous),
                    new DecisionVariable("Instrumentalness",        DecisionVariableKind.Continuous),
                    new DecisionVariable("Liveness",        DecisionVariableKind.Continuous),
                    new DecisionVariable("Loudness",        DecisionVariableKind.Continuous),
                    new DecisionVariable("Speechiness",        DecisionVariableKind.Continuous),
                    new DecisionVariable("Tempo",        DecisionVariableKind.Continuous),
                    new DecisionVariable("Valence",        DecisionVariableKind.Continuous)
            };

            // Use the learning algorithm to induce a new tree:
            DecisionTree tree = teacher.Learn(inputs, outputs);

            // And then predict the label using
            var predicted = tree.Decide(test); 

            // We can translate it back to strings using
            var answer = codebook.Revert("Target", predicted); 

            return answer;       
        }

    }
}
