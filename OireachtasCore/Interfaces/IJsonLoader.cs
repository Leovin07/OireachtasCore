namespace OireachtasAPI
{
    public interface IJsonLoader
    {
        /// <summary>
        /// Loads JSON data from a file.
        /// </summary>
        /// <param name="filename">The path to the JSON file.</param>
        /// <returns>The loaded JSON data.</returns>
        Task<dynamic> LoadJson(string filename);

        /// <summary>
        /// Loads JSON data from a string.
        /// </summary>
        /// <param name="input">The JSON string.</param>
        /// <returns>The loaded JSON data.</returns>
        Task<dynamic> LoadJsonData(string input);
    }
}