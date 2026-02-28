using CarComparator.Modules.Cars.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace CarComparator.Modules.Scraping.Services;

public class CarScraperService : ICarScraperService
{
    private readonly ILogger<CarScraperService> _logger;

    public CarScraperService(ILogger<CarScraperService> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<Car>> ScrapeAsync(string sourceUrl, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting scraping of {Url}", sourceUrl);
        var cars = new List<Car>();

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        var page = await browser.NewPageAsync();

        try
        {
            await page.GotoAsync(sourceUrl, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle,
                Timeout = 30000
            });

            // Generic scraping logic - adapt selectors per target site
            var carElements = await page.QuerySelectorAllAsync("[data-car-item], .car-item, .listing-item, article.car");

            if (carElements.Count == 0)
            {
                _logger.LogWarning("No car elements found on {Url} with generic selectors", sourceUrl);
                return cars;
            }

            foreach (var element in carElements)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    var car = await ExtractCarFromElementAsync(element, sourceUrl);
                    if (car is not null)
                        cars.Add(car);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to extract car data from element");
                }
            }
        }
        finally
        {
            await page.CloseAsync();
        }

        _logger.LogInformation("Scraped {Count} cars from {Url}", cars.Count, sourceUrl);
        return cars;
    }

    private static async Task<Car?> ExtractCarFromElementAsync(IElementHandle element, string sourceUrl)
    {
        var titleEl = await element.QuerySelectorAsync(".title, h2, h3, [data-make]");
        var priceEl = await element.QuerySelectorAsync(".price, [data-price], .car-price");
        var imgEl = await element.QuerySelectorAsync("img");
        var linkEl = await element.QuerySelectorAsync("a");

        if (titleEl is null) return null;

        var titleText = await titleEl.InnerTextAsync();
        var priceText = priceEl is not null ? await priceEl.InnerTextAsync() : null;
        var imageUrl = imgEl is not null ? await imgEl.GetAttributeAsync("src") : null;
        var linkHref = linkEl is not null ? await linkEl.GetAttributeAsync("href") : null;

        if (string.IsNullOrWhiteSpace(titleText)) return null;

        var (make, model, year) = ParseTitle(titleText);
        var price = ParsePrice(priceText);

        return new Car
        {
            Make = make,
            Model = model,
            Year = year,
            Price = price,
            ImageUrl = imageUrl,
            SourceUrl = linkHref ?? sourceUrl,
            CreatedAt = DateTime.UtcNow
        };
    }

    private static (string make, string model, int year) ParseTitle(string title)
    {
        var parts = title.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        int year = DateTime.UtcNow.Year;
        string make = "Unknown";
        string model = "Unknown";

        int startIndex = 0;
        if (parts.Length > 0 && int.TryParse(parts[0], out int parsedYear) && parsedYear > 1900 && parsedYear <= DateTime.UtcNow.Year + 1)
        {
            year = parsedYear;
            startIndex = 1;
        }

        if (parts.Length > startIndex)
            make = parts[startIndex];

        if (parts.Length > startIndex + 1)
            model = string.Join(" ", parts[(startIndex + 1)..]);

        return (make, model, year);
    }

    private static decimal ParsePrice(string? priceText)
    {
        if (string.IsNullOrWhiteSpace(priceText)) return 0;

        var cleaned = new string(priceText.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray())
            .Replace(",", "");

        return decimal.TryParse(cleaned, out var price) ? price : 0;
    }
}
