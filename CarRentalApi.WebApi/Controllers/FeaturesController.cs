using CarRentalApi.Business.Operations.Car.CarDtos;
using CarRentalApi.Business.Operations.Car;
using CarRentalApi.Business.Operations.Feature;
using CarRentalApi.Business.Operations.Feature.Dtos;
using CarRentalApi.WebApi.Filters;
using CarRentalApi.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    { 
        private readonly IFeatureService _featureService;

        public FeaturesController(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddFeature(AddFeatureRequest request)
        {
            var addFeatureDto = new AddFeatureDto
            {
                Title = request.Title,
            };

            var result  = await _featureService.AddFeature(addFeatureDto);

            if(result.IsSucceed)
                return Ok(result.Message);
            else
                return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            var result = await _featureService.DeleteFeature(id);

            if (!result.IsSucceed)
                return NotFound(result.Message);
            else
                return Ok(result.Message);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFeature(int id, UpdateFeatureRequest request)
        {
            var updateFeatureDto = new UpdateFeatureDto
            {
                Id = id,
                Title = request.Title,
            };

            var result = await _featureService.UpdateFeature(updateFeatureDto);

            if (!result.IsSucceed)
                return NotFound(result.Message);
            else
                return Ok(result.Message);
        }
    }
}
