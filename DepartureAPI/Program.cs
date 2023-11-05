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

app.MapGet("/stopdepatures/{stopId}", (int stopId) =>
{
    var list = GTFSActions.GTFSActions.Get5StopDepartureStrings(feed, stopId);
    return list;
});

app.Run();