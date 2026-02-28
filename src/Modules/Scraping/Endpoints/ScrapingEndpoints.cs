using CarComparator.Modules.Cars.Services;
using CarComparator.Modules.Scraping.Services;
using CarComparator.Shared.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CarComparator.Modules.Scraping.Endpoints;

public static class ScrapingEndpoints
{
    public static IEndpointRouteBuilder MapScrapingEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/cars").WithTags("Scraping");

        group.MapPost("/scrape", async (
            ScrapeRequestDto request,
            ICarScraperService scraperService,
            ICarsService carsService) =>
        {
            if (string.IsNullOrWhiteSpace(request.SourceUrl))
                return Results.BadRequest("SourceUrl is required.");

            var cars = await scraperService.ScrapeAsync(request.SourceUrl);
            var count = await carsService.AddCarsAsync(cars);

            return Results.Ok(new { Indexed = count });
        })
        .WithName("ScrapeCars")
        .WithOpenApi();

        return endpoints;
    }
}
