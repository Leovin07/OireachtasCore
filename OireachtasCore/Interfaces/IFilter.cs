namespace OireachtasAPI
{
    public interface IFilter
    {
        /// <summary>
        /// Filters the data based on a specific criteria.
        /// </summary>
        /// <param name="data">The data to be filtered.</param>
        /// <returns>The filtered data.</returns>
        Task<IEnumerable<dynamic>> Filter(dynamic data);
    }
}