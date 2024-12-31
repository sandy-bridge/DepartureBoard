namespace GTFSActions;
using GTFS;
using GTFS.Entities;
using static System.DateTime;
using System.IO.Compression;
using System.Collections;
using System.Reflection.Metadata.Ecma335;

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
    private static bool CheckTripDate (GTFSFeed feed, string trip)
    {
        if(feed.Calendars.First(c => c.ServiceId == feed.Trips.Get(trip).ServiceId).CoversDate(Today) && feed.Calendars.First(c => c.ServiceId == feed.Trips.Get(trip).ServiceId).ContainsDay(Now.DayOfWeek))
        {
            if(feed.CalendarDates.FirstOrDefault(c => c.Date == Today && c.ServiceId == feed.Trips.Get(trip).ServiceId)?.ExceptionType == GTFS.Entities.Enumerations.ExceptionType.Removed)
            {
                return false;
            }
            else return true;
        }
        if(feed.CalendarDates.FirstOrDefault(c => c.Date == Today && c.ServiceId == feed.Trips.Get(trip).ServiceId)?.ExceptionType == GTFS.Entities.Enumerations.ExceptionType.Added)
        {
            return true;
        }
        else {
            return false;
        }
    }
    public static List<string> GetMultipleStopsDepartureStrings (GTFSFeed gtfsFeed, List<int> stops, int n=5)
    {
        var departureLists = new List<List<StopTime>>();
        stops.ForEach(s => departureLists.Add(gtfsFeed.StopTimes.GetForStop(s.ToString())
            .Where(t => t.DepartureTime > TimeOfDay.FromDateTime(Now))
            .ToList()));
        var filteredDepartures = new List<List<StopTime>>();
        foreach (var departureList in departureLists){
            filteredDepartures.Add(departureList.Where(s => CheckTripDate(gtfsFeed, s.TripId)).ToList());
        }
        filteredDepartures.ForEach(d => d.Sort(
            (a, b) => (int)a.DepartureTime?.CompareTo(b.DepartureTime)!));
        var departureQueueList = new List<Queue<StopTime>>();
        filteredDepartures.ForEach(d => departureQueueList.Add(new Queue<StopTime>(d)));
        var combinedList = new List<StopTime>();
        while(departureQueueList.Any(s => s.Count > 0)){
            combinedList.Add(departureQueueList.Aggregate((curMin, x) => (curMin == null || (x.Peek().DepartureTime) <
    curMin.Peek().DepartureTime ? x : curMin)).Dequeue());
            departureQueueList.RemoveAll(q => q.Count == 0);
        }
        List<String> list = new List<string>();
        foreach (var time in combinedList.Take(n))
        {
            var trip = gtfsFeed.Trips.FirstOrDefault(t => t.Id == time.TripId);
            var route = gtfsFeed.Routes.FirstOrDefault(r => r.Id == trip?.RouteId);
            var departureTime = time.DepartureTime?.ToString().Substring(0, 5);
            list.Add(String.Format(departureTime + " " + route?.ShortName + " " + trip?.Headsign + " " + trip?.Id));
        }

        return list;
    }
    public static List<string> GetStopDepartureStrings(GTFSFeed gtfsFeed, int stopId, int n=5)
    {
        var departures = gtfsFeed.StopTimes.GetForStop(stopId.ToString())
            .Where(t => t.DepartureTime > TimeOfDay.FromDateTime(Now))
            .ToList();
        departures.Sort((a, b) => (int)a.DepartureTime?.CompareTo(b.DepartureTime)!);
        List<String> list = new List<string>();
        foreach (var time in departures.Take(n))
        {
            var trip = gtfsFeed.Trips.FirstOrDefault(t => t.Id == time.TripId);
            var route = gtfsFeed.Routes.FirstOrDefault(r => r.Id == trip?.RouteId);
            var departureTime = time.DepartureTime?.ToString().Substring(0, 5);
            list.Add(String.Format(departureTime + " " + route?.ShortName + " " + trip?.Headsign +  " " + trip?.Id ));
        }

        return list;
    }
}
