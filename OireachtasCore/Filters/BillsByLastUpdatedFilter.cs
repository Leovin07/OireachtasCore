namespace OireachtasAPI
{
    public class BillsByLastUpdatedFilter : Filters
    {
        private readonly DateTime _since;
        private readonly DateTime _until;

        public BillsByLastUpdatedFilter(DateTime since, DateTime until)
        {
            _since = since;
            _until = until;
        }

        /// <summary>
        /// Filters the data to retrieve bills updated within a specific date range.
        /// </summary>
        /// <param name="data">The data to be filtered.</param>
        /// <returns>The filtered data.</returns>
        public override async Task<IEnumerable<dynamic>> Filter(dynamic data)
        {
            if (data == null || !data.Any())
            {
                return Enumerable.Empty<dynamic>();
            }

            List<dynamic> updatedBills = new List<dynamic>();

            await Task.Run(() =>
            {
                Parallel.ForEach(data as IEnumerable<dynamic>, result =>
                {
                    DateTime lastUpdated;
                    if (DateTime.TryParse(result.bill.lastUpdated.ToString(), out lastUpdated))
                    {
                        if (lastUpdated >= _since && lastUpdated <= _until.AddDays(-1))
                        {
                            updatedBills.Add(result.bill);
                        }
                    }
                });
            }).ConfigureAwait(false);

            return updatedBills;
        }
    }
}