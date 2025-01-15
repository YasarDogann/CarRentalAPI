using CarRentalApi.Business.Operations.Car.CarDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Business.Operations.Feature.Dtos
{
    public class FeatureDto
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public List<CarDto> Cars { get; set; }
    }
}
