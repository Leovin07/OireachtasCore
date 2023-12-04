namespace OireachtasAPI
{
    public interface IDataLoader
    {
        /// <summary>
        /// Loads the data from a source.
        /// </summary>
        /// <returns>The loaded data.</returns>
        Task<dynamic> LoadData();
    }
}