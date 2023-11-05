var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

const string GTFS_URI = "https://peatus.ee/gtfs/gtfs.zip";
var feed = await GTFSActions.GTFSActions.DownloadData(GTFS_URI);

app.MapGet("/stopdepatures/{stopId}", (string stopId) =>
{
    var stopName = feed.Stops.FirstOrDefault(s=> s.Id == stopId)?.Name ?? null;
    if(stopName == null){
        return Results.NotFound();
    }
    var list = GTFSActions.GTFSActions.Get5StopDepartureStrings(feed, int.Parse(stopId));
    return Results.Json(new { StopName = stopName, Departures = list});
});

app.Run();