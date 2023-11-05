﻿const String GTFS_URI = "https://peatus.ee/gtfs/gtfs.zip";
var feed = await GTFSActions.GTFSActions.DownloadData(GTFS_URI);
var test_stop_id = 908;
Console.WriteLine("Stop ID " + test_stop_id + ": " + feed.Stops.FirstOrDefault(s=> int.Parse(s.Id) == test_stop_id)?.Name);
var departureStrings = GTFSActions.GTFSActions.Get5StopDepartureStrings(feed, test_stop_id);
foreach (var departureString in departureStrings)
{
    Console.WriteLine(departureString);
}
