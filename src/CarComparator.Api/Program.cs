using CarComparator.Modules.Cars;
using CarComparator.Modules.Cars.Endpoints;
using CarComparator.Modules.Scraping;
using CarComparator.Modules.Scraping.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=carcomparator.db";

builder.Services.AddCarsModule(connectionString);
builder.Services.AddScrapingModule();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(
                builder.Configuration.GetValue<string>("AllowedOrigins") ?? "http://localhost:5001")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

await CarComparator.Modules.Cars.CarsModule.MigrateCarsModuleAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();

app.MapCarsEndpoints();
app.MapScrapingEndpoints();

app.Run();
