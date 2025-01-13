using CarRentalApi.Business.Operations.Car.CarDtos;
using CarRentalApi.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Business.Operations.Car
{
    public interface ICarService
    {
        Task<ServiceMessage> AddCar(AddCarDto car);
        Task<CarDto> GetCar(int id);
        Task<List<CarDto>> GetAllCars();
        Task<ServiceMessage> AdjustCarPrice(int id, decimal changeBy);
        Task<ServiceMessage> DeleteCar(int id);
        Task<ServiceMessage> UpdateCar(UpdateCarDto car);
    }
}
