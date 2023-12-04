namespace OireachtasAPI
{
    public abstract class Filters : IFilter
    {
        /// <summary>
        /// Filters the data based on a specific criteria.
        /// </summary>
        /// <param name="data">The data to be filtered.</param>
        /// <returns>The filtered data.</returns>
        public abstract Task<IEnumerable<dynamic>> Filter(dynamic data);
    }
}