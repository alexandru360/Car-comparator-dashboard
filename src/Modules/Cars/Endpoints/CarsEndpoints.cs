using CarComparator.Modules.Cars.Services;
using CarComparator.Shared.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CarComparator.Modules.Cars.Endpoints;

public static class CarsEndpoints
{
    public static IEndpointRouteBuilder MapCarsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/cars").WithTags("Cars");

        group.MapGet("/", async (
            string? make,
            string? model,
            int? yearFrom,
            int? yearTo,
            decimal? priceFrom,
            decimal? priceTo,
            string? fuelType,
            string? transmission,
            int? mileageMax,
            int page,
            int pageSize,
            ICarsService carsService) =>
        {
            var search = new CarSearchDto
            {
                Make = make,
                Model = model,
                YearFrom = yearFrom,
                YearTo = yearTo,
                PriceFrom = priceFrom,
                PriceTo = priceTo,
                FuelType = fuelType,
                Transmission = transmission,
                MileageMax = mileageMax,
                Page = page > 0 ? page : 1,
                PageSize = pageSize > 0 ? Math.Min(pageSize, 100) : 20
            };
            var result = await carsService.SearchAsync(search);
            return Results.Ok(result);
        })
        .WithName("GetCars")
        .WithOpenApi();

        group.MapGet("/{id:int}", async (int id, ICarsService carsService) =>
        {
            var car = await carsService.GetByIdAsync(id);
            return car is null ? Results.NotFound() : Results.Ok(car);
        })
        .WithName("GetCarById")
        .WithOpenApi();

        return endpoints;
    }
}
