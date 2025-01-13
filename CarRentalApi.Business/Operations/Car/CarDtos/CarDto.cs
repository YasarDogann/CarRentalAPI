using CarRentalApi.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Business.Operations.Car.CarDtos
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public int StockQuantity { get; set; }

        public bool IsInStock => StockQuantity > 0;

        public VehicleType VehicleType { get; set; }
        public List<CarFeaturesDto> Features { get; set; }
    }
}
