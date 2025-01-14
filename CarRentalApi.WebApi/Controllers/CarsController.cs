using CarRentalApi.Business.Excepions;
using CarRentalApi.Business.Operations.Car;
using CarRentalApi.Business.Operations.Car.CarDtos;
using CarRentalApi.WebApi.Filters;
using CarRentalApi.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace CarRentalApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCar(int id)
        {
            var car = await _carService.GetCar(id);

            if (car is null)
            {
                // return NotFound();
                throw new NotFoundException($"{id} id'li araç bulunamadı");
            }
            else
            {
                return Ok(car);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carService.GetAllCars();
            return Ok(cars);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCar(AddCarRequest request) // [FromBody]
        {
            var addCarDto = new AddCarDto
            {
                Make = request.Make,
                Model = request.Model,
                Year = (int)request.Year,
                PricePerDay = request.PricePerDay,
                StockQuantity = request.StockQuantity,
                FeatureIds = request.FeatureIds,
                VehicleType = request.VehicleType,
            };

            var result = await _carService.AddCar(addCarDto);

            return Ok(result.Message);
            /*
            if (!result.IsSucceed)
            {
                return BadRequest(result.Message);
            }
            else
            {
                return Ok(result.Message);
            }
            */
        }

        [HttpPatch("{id}/PricePerDay")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdjustCarPrice(int id, decimal changeBy)
        {
            var result = await _carService.AdjustCarPrice(id, changeBy);

            return Ok(result.Message); // todo : get yönlendirme yapılabilir
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var result = await _carService.DeleteCar(id);

            return Ok(result.Message);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [TimeControlFilter]
        public async Task<IActionResult> UpdateCar(int id, UpdateCarRequest request)
        {
            var updateCarDto = new UpdateCarDto
            {
                Id = id,
                Make = request.Make,
                Model = request.Model,
                Year = (int)request.Year,
                PricePerDay = request.PricePerDay,
                StockQuantity = request.StockQuantity,
                VehicleType = request.VehicleType,
                FeatureIds = request.FeatureIds,
            };

            var result = await _carService.UpdateCar(updateCarDto);

            return await GetCar(id);
        }
    }
}
