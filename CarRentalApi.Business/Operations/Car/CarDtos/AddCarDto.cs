using CarRentalApi.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Business.Operations.Car.CarDtos
{
    public class AddCarDto
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public int StockQuantity { get; set; }
        public bool IsInStock => StockQuantity > 0;
        public VehicleType VehicleType { get; set; }
        public List<int> FeatureIds { get; set; }
    }
}
