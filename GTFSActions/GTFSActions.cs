namespace GTFSActions;
using GTFS;
using GTFS.Entities;
using static System.DateTime;
using System.IO.Compression;
public class GTFSActions
{
    public static async Task<GTFSFeed> DownloadData(string gtfs_uri, bool overwrite = false)
    {
        var reader = new GTFSReader<GTFSFeed>();
        if (File.Exists("gtfs/feed_info.txt") && overwrite == false)
        {
            if (File.GetLastWriteTime("gtfs/feed_info.txt") > Now.AddDays(-7))
            {
                Console.WriteLine("Using existing data.");
                return reader.Read("gtfs");
            }
        }

        if (File.Exists("gtfs.zip"))
        {
            File.Delete("gtfs.zip");
        }
        if (Directory.Exists("gtfs"))
        {
            Directory.Delete("gtfs");
        }
        Console.WriteLine("Downloading public transport data.");

        await HttpDownloadAndUnzip(gtfs_uri, "gtfs");
        return reader.Read("gtfs");

        async Task<bool> HttpDownloadAndUnzip(string requestUri, string directoryToUnzip)
        {
            using var response = await new HttpClient().GetAsync(requestUri);
            if (!response.IsSuccessStatusCode) return false;

            using var streamToReadFrom = await response.Content.ReadAsStreamAsync();
            using var zip = new ZipArchive(streamToReadFrom);
            zip.ExtractToDirectory(directoryToUnzip);
            return true;
        }
    }
    public static List<string> Get5StopDepartureStrings(GTFSFeed gtfsFeed, int stopId)
    {
        var departures = gtfsFeed.StopTimes.GetForStop(stopId.ToString())
            .Where(t => t.DepartureTime > TimeOfDay.FromDateTime(Now))
            .ToList();
        departures.Sort((a, b) => (int)a.DepartureTime?.CompareTo(b.DepartureTime)!);
        List<String> list = new List<string>();
        foreach (var time in departures.Take(5))
        {
            var trip = gtfsFeed.Trips.FirstOrDefault(t => t.Id == time.TripId);
            var route = gtfsFeed.Routes.FirstOrDefault(r => r.Id == trip?.RouteId);
            var departureTime = time.DepartureTime?.ToString().Substring(0, 5);
            list.Add(String.Format(departureTime + " " + route?.ShortName + " " + trip?.Headsign));
        }

        return list;
    }
}
