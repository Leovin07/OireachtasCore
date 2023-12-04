namespace OireachtasAPI
{
    public class BillsSponsoredByFilter : Filters
    {
        private readonly string _sponsorId;

        public BillsSponsoredByFilter(string sponsorId)
        {
            _sponsorId = sponsorId;
        }

        /// <summary>
        /// Filters the data to retrieve bills sponsored by a specific sponsor.
        /// </summary>
        /// <param name="data">The data to be filtered.</param>
        /// <returns>The filtered data.</returns>
        public override async Task<IEnumerable<dynamic>> Filter(dynamic data)
        {
            if (data == null || !data.Any())
            {
                return Enumerable.Empty<dynamic>();
            }

            List<dynamic> sponsoredBills = new List<dynamic>();

            await Task.Run(() =>
            {
                Parallel.ForEach(data as IEnumerable<dynamic>, result =>
                {
                    dynamic sponsors = result.bill.sponsors;
                    foreach (dynamic sponsor in sponsors)
                    {
                        string sponsorName = sponsor.sponsor.by.showAs;
                        if (_sponsorId == null || sponsorName.ToString() == _sponsorId.ToString())
                        {
                            sponsoredBills.Add(result.bill);
                            break;
                        }
                    }
                });
            }).ConfigureAwait(false);

            return sponsoredBills;
        }
    }
}

