using CarComparator.Modules.Cars.Data;
using CarComparator.Modules.Cars.Entities;
using CarComparator.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CarComparator.Modules.Cars.Services;

public class CarsService : ICarsService
{
    private readonly CarsDbContext _db;

    public CarsService(CarsDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResultDto<CarListItemDto>> SearchAsync(CarSearchDto search)
    {
        var query = _db.Cars.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.Make))
            query = query.Where(c => c.Make.ToLower().Contains(search.Make.ToLower()));

        if (!string.IsNullOrWhiteSpace(search.Model))
            query = query.Where(c => c.Model.ToLower().Contains(search.Model.ToLower()));

        if (search.YearFrom.HasValue)
            query = query.Where(c => c.Year >= search.YearFrom.Value);

        if (search.YearTo.HasValue)
            query = query.Where(c => c.Year <= search.YearTo.Value);

        if (search.PriceFrom.HasValue)
            query = query.Where(c => c.Price >= search.PriceFrom.Value);

        if (search.PriceTo.HasValue)
            query = query.Where(c => c.Price <= search.PriceTo.Value);

        if (!string.IsNullOrWhiteSpace(search.FuelType))
            query = query.Where(c => c.FuelType == search.FuelType);

        if (!string.IsNullOrWhiteSpace(search.Transmission))
            query = query.Where(c => c.Transmission == search.Transmission);

        if (search.MileageMax.HasValue)
            query = query.Where(c => c.Mileage == null || c.Mileage <= search.MileageMax.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(c => new CarListItemDto
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model,
                Year = c.Year,
                Price = c.Price,
                FuelType = c.FuelType,
                Transmission = c.Transmission,
                Mileage = c.Mileage,
                ImageUrl = c.ImageUrl,
                SourceUrl = c.SourceUrl
            })
            .ToListAsync();

        return new PagedResultDto<CarListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = search.Page,
            PageSize = search.PageSize
        };
    }

    public async Task<CarDto?> GetByIdAsync(int id)
    {
        var car = await _db.Cars.FindAsync(id);
        if (car is null) return null;

        return new CarDto
        {
            Id = car.Id,
            Make = car.Make,
            Model = car.Model,
            Year = car.Year,
            Price = car.Price,
            FuelType = car.FuelType,
            Transmission = car.Transmission,
            Mileage = car.Mileage,
            Color = car.Color,
            Description = car.Description,
            ImageUrl = car.ImageUrl,
            SourceUrl = car.SourceUrl,
            CreatedAt = car.CreatedAt
        };
    }

    public async Task<int> AddCarAsync(Car car)
    {
        _db.Cars.Add(car);
        await _db.SaveChangesAsync();
        return car.Id;
    }

    public async Task<int> AddCarsAsync(IEnumerable<Car> cars)
    {
        var carList = cars.ToList();
        _db.Cars.AddRange(carList);
        await _db.SaveChangesAsync();
        return carList.Count;
    }
}
