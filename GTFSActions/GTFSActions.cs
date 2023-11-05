namespace GTFSActions;
using GTFS;
using GTFS.Entities;
using static System.DateTime;
public class GTFSActions
{
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
