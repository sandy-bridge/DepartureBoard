using System.IO.Compression;
using GTFS;
using static System.DateTime;



async Task<GTFSFeed> DownloadData(string gtfs_uri, bool overwrite = false)
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

const String GTFS_URI = "https://peatus.ee/gtfs/gtfs.zip";
var feed = await DownloadData(GTFS_URI);
var test_stop_id = 908;
Console.WriteLine("Stop ID " + test_stop_id + ": " + feed.Stops.FirstOrDefault(s=> int.Parse(s.Id) == test_stop_id)?.Name);
var departureStrings = GTFSActions.GTFSActions.Get5StopDepartureStrings(feed, test_stop_id);
foreach (var departureString in departureStrings)
{
    Console.WriteLine(departureString);
}
