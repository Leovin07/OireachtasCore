using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OireachtasAPI
{

    public class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("Kindly provide a filter message ( one of filterBillsSponsoredBy, filterBillsByLastUpdated) and input arguments.");
                    return;
                }

                string filterMessage = args[0];
                string[] filterArgs = args.Skip(1).ToArray();

                IJsonLoader jsonLoader = new JsonLoader();
                dynamic data = await jsonLoader.LoadJsonData(filterArgs[0]).ConfigureAwait(false);

                switch (filterMessage)
                {
                    case "filterBillsSponsoredBy":
                        await FilterBillsSponsoredBy(filterArgs, data).ConfigureAwait(false);
                        break;
                    case "filterBillsByLastUpdated":
                        await FilterBillsByLastUpdated(filterArgs, data).ConfigureAwait(false);
                        break;
                    default:
                        throw new ArgumentException("Invalid filter message.");
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("File not found: " + ex.FileName);
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Error deserializing JSON: " + ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid argument: " + ex.ParamName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        /// <summary>
        /// Filters the bills based on the sponsor ID.
        /// </summary>
        /// <param name="filterArgs">The filter arguments.</param>
        /// <param name="data">The data to be filtered.</param>
        public static async Task FilterBillsSponsoredBy(string[] filterArgs, dynamic data)
        {
            if (data == null || !data.Any())
            {
                Console.WriteLine("No data available.");
                return;
            }

            string sponsorId = null;
            if (filterArgs.Length >= 1)
            {
                sponsorId = filterArgs[0];
            }
            IFilter sponsoredByFilter = new BillsSponsoredByFilter(sponsorId);
            IEnumerable<dynamic> sponsoredBills = await sponsoredByFilter.Filter(data).ConfigureAwait(false);
            await PrintBills(sponsoredBills).ConfigureAwait(false);
        }

        /// <summary>
        /// Filters the bills based on the last updated date.
        /// </summary>
        /// <param name="filterArgs">The filter arguments.</param>
        /// <param name="data">The data to be filtered.</param>
        public static async Task FilterBillsByLastUpdated(string[] filterArgs, dynamic data)
        {
            if (data == null || !data.Any())
            {
                Console.WriteLine("No data available.");
                return;
            }

            if (filterArgs.Length < 1 || filterArgs.Length > 2)
            {
                Console.WriteLine("Invalid number of filter arguments.");
                return;
            }

            DateTime since;
            DateTime until = DateTime.Today;

            if (!DateTime.TryParse(filterArgs[0], out since))
            {
                Console.WriteLine("Invalid 'since' date format.");
                return;
            }

            if (filterArgs.Length == 2 && !DateTime.TryParse(filterArgs[1], out until))
            {
                Console.WriteLine("Invalid 'until' date format.");
                return;
            }

            if (filterArgs.Length == 1)
            {
                until = DateTime.Today;
            }

            IFilter lastUpdatedFilter = new BillsByLastUpdatedFilter(since, until);
            IEnumerable<dynamic> updatedBills = await lastUpdatedFilter.Filter(data).ConfigureAwait(false);
            await PrintBills(updatedBills).ConfigureAwait(false);
        }

        /// <summary>
        /// Prints the bills to the console.
        /// </summary>
        /// <param name="bills">The bills to be printed.</param>
        public static async Task PrintBills(IEnumerable<dynamic> bills)
        {
            if (bills == null)
            {
                throw new ArgumentNullException(nameof(bills));
            }

            if (!bills.Any())
            {
                Console.WriteLine("No bills found.");
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (dynamic bill in bills)
            {
                sb.AppendLine(bill.ToString());
            }

            await Console.Out.WriteLineAsync(sb.ToString()).ConfigureAwait(false);
        }
    }
}