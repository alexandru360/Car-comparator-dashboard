using CarComparator.Modules.Cars.Data;
using CarComparator.Modules.Cars.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarComparator.Modules.Cars;

public static class CarsModule
{
    public static IServiceCollection AddCarsModule(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CarsDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<ICarsService, CarsService>();

        return services;
    }

    public static async Task MigrateCarsModuleAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CarsDbContext>();
        await db.Database.MigrateAsync();
    }
}
