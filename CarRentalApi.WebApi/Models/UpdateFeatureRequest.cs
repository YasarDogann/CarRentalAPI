using System.ComponentModel.DataAnnotations;

namespace CarRentalApi.WebApi.Models
{
    public class UpdateFeatureRequest
    {
        [Required]
        public string Title { get; set; }
    }
}
