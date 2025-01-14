using CarRentalApi.Business.Operations.Car.CarDtos;
using CarRentalApi.Business.Operations.Car;
using CarRentalApi.Business.Operations.Feature;
using CarRentalApi.Business.Operations.Feature.Dtos;
using CarRentalApi.WebApi.Filters;
using CarRentalApi.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarRentalApi.Business.Operations.User;
using CarRentalApi.Business.Excepions;

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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllFeatures()
        {
            var features = await _featureService.GetAllFeatures();
            return Ok(features);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddFeature(AddFeatureRequest request)
        {
            var addFeatureDto = new AddFeatureDto
            {
                Title = request.Title,
            };

            var result = await _featureService.AddFeature(addFeatureDto);

            return Ok(result.Message);

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            var result = await _featureService.DeleteFeature(id);
            return Ok(result.Message);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFeature(int id, UpdateFeatureRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new BadRequestException(string.Join(", ", errors));
            }

            var updateFeatureDto = new UpdateFeatureDto
            {
                Id = id,
                Title = request.Title,
            };

            var result = await _featureService.UpdateFeature(updateFeatureDto);

            return Ok(result.Message);
        }
    }
}
