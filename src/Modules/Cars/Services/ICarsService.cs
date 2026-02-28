using CarComparator.Modules.Cars.Entities;
using CarComparator.Shared.DTOs;

namespace CarComparator.Modules.Cars.Services;

public interface ICarsService
{
    Task<PagedResultDto<CarListItemDto>> SearchAsync(CarSearchDto search);
    Task<CarDto?> GetByIdAsync(int id);
    Task<int> AddCarAsync(Car car);
    Task<int> AddCarsAsync(IEnumerable<Car> cars);
}
