using CarComparator.Modules.Scraping.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarComparator.Modules.Scraping;

public static class ScrapingModule
{
    public static IServiceCollection AddScrapingModule(this IServiceCollection services)
    {
        services.AddScoped<ICarScraperService, CarScraperService>();
        return services;
    }
}
