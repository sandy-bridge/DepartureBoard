var builder = WebApplication.CreateBuilder(args);
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
    policy =>
    {
        policy.WithOrigins("http://localhost:3000");
    });
});

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

app.MapGet("/stopdepartures/{stopId}", (string stopId) =>
{
    var stopName = feed.Stops.FirstOrDefault(s=> s.Id == stopId)?.Name ?? null;
    if(stopName == null){
        return Results.NotFound();
    }
    var list = GTFSActions.GTFSActions.Get5StopDepartureStrings(feed, int.Parse(stopId));
    return Results.Json(new { StopName = stopName, Departures = list});
});

app.UseCors(MyAllowSpecificOrigins);

app.Run();
