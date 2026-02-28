using CarComparator.Modules.Cars.Entities;

namespace CarComparator.Modules.Scraping.Services;

public interface ICarScraperService
{
    Task<IEnumerable<Car>> ScrapeAsync(string sourceUrl, CancellationToken cancellationToken = default);
}
