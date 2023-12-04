using System.Text.Json;

namespace OireachtasAPI
{
    public class JsonLoader : IJsonLoader
    {
        private readonly Dictionary<string, Tuple<dynamic, DateTime>> _cache;

        public JsonLoader()
        {
            _cache = new Dictionary<string, Tuple<dynamic, DateTime>>();
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        public void ClearCache()
        {
            _cache.Clear();
        }

        /// <summary>
        /// Loads JSON data from a file.
        /// </summary>
        /// <param name="filename">The path to the JSON file.</param>
        /// <returns>The loaded JSON data.</returns>
        public async Task<dynamic> LoadJson(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }

            if (File.Exists(filename))
            {
                using (StreamReader file = File.OpenText(filename))
                {
                    var options = new JsonSerializerOptions();
                    return await JsonSerializer.DeserializeAsync<dynamic>(file.BaseStream, options).ConfigureAwait(false);
                }
            }
            else
            {
                throw new FileNotFoundException("File not found: " + filename);
            }
        }

        /// <summary>
        /// Loads JSON data from a string.
        /// </summary>
        /// <param name="input">The JSON string.</param>
        /// <returns>The loaded JSON data.</returns>
        public async Task<dynamic> LoadJsonData(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (_cache.ContainsKey(input) && !IsCacheExpired(input))
            {
                return _cache[input].Item1;
            }

            dynamic jsonData;

            if (Uri.TryCreate(input, UriKind.Absolute, out Uri uri))
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(input).ConfigureAwait(false);
                    var options = new JsonSerializerOptions();
                    jsonData = await JsonSerializer.DeserializeAsync<dynamic>(await response.Content.ReadAsStreamAsync().ConfigureAwait(false), options).ConfigureAwait(false);
                }
            }
            else
            {
                jsonData = await LoadJson(input).ConfigureAwait(false);
            }

            _cache[input] = Tuple.Create(jsonData, DateTime.Now.AddMinutes(10)); // Set expiry time to 10 minutes
            return jsonData;
        }

        private bool IsCacheExpired(string input)
        {
            return DateTime.Now > _cache[input].Item2;
        }
    }
}