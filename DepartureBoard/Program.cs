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

var httpClient = new HttpClient();
Console.WriteLine("Downloading public transport data.");
const string GTFS_URI = "https://peatus.ee/gtfs/gtfs.zip";
var httpResult = await httpClient.GetAsync(GTFS_URI);
await using var resultStream = await httpResult.Content.ReadAsStreamAsync();
await using var fileStream = File.Create("gtfs.zip");
resultStream.CopyTo(fileStream);
await fileStream.DisposeAsync();
var reader = new GTFSReader<GTFSFeed>();
System.IO.Compression.ZipFile.ExtractToDirectory("gtfs.zip", "gtfs", true);
var feed = reader.Read("gtfs");
var test_stop_id = 908;
Console.WriteLine("Stop ID " + test_stop_id + ": " + feed.Stops.FirstOrDefault(s=> int.Parse(s.Id) == test_stop_id)?.Name);
var departureStrings = Get5StopDepartureStrings(feed, test_stop_id);
foreach (var departureString in departureStrings)
{
    Console.WriteLine(departureString);
}
