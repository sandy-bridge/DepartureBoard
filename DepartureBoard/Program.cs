using System.IO.Compression;
using GTFS;
using GTFS.Entities;
using GTFS.IO;
using static System.DateTime;

List<string> Get5StopDepartureStrings(GTFSFeed gtfsFeed, int stopId)
{
    var departures = gtfsFeed.StopTimes.GetForStop(stopId.ToString())
        .Where(t => t.DepartureTime > TimeOfDay.FromDateTime(Now))
        .ToList();
    departures.Sort((a, b) => (int) a.DepartureTime?.CompareTo(b.DepartureTime)!);
    List<String> list = new List<string>();
    foreach (var time in departures.Take(5))
    {
        var trip = gtfsFeed.Trips.FirstOrDefault(t => t.Id == time.TripId);
        var route = gtfsFeed.Routes.FirstOrDefault(r => r.Id == trip?.RouteId);
        list.Add(String.Format(time.DepartureTime + " " + route?.ShortName + " " + trip?.Headsign));
    }

    return list;
}

async Task<GTFSFeed> DownloadData(string gtfs_uri, bool overwrite = false)
{
    var reader = new GTFSReader<GTFSFeed>();
    if (File.Exists("gtfs/feed_info.txt") && overwrite == false)
    {
        if (File.GetLastWriteTime("gtfs/feed_info.txt") > Now.AddDays(-7))
        {
            return reader.Read("gtfs");
        }
    }

    if (File.Exists("gtfs.zip"))
    {
        File.Delete("gtfs.zip");
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

const String GTFS_URI = "https://peatus.ee/gtfs";
var feed = await DownloadData(GTFS_URI);
var test_stop_id = 908;
Console.WriteLine("Stop ID " + test_stop_id + ": " + feed.Stops.FirstOrDefault(s=> int.Parse(s.Id) == test_stop_id)?.Name);
var departureStrings = Get5StopDepartureStrings(feed, test_stop_id);
foreach (var departureString in departureStrings)
{
    Console.WriteLine(departureString);
}
