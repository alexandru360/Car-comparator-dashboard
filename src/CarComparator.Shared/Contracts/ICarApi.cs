using CarComparator.Shared.DTOs;
using Refit;

namespace CarComparator.Shared.Contracts;

public interface ICarApi
{
    [Get("/api/cars")]
    Task<PagedResultDto<CarListItemDto>> GetCarsAsync([Query] CarSearchDto search);

    [Get("/api/cars/{id}")]
    Task<CarDto> GetCarByIdAsync(int id);

    [Post("/api/cars/scrape")]
    Task ScrapeAsync([Body] ScrapeRequestDto request);
}
