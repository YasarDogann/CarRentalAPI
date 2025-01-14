using CarRentalApi.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarRentalApi.WebApi.Models
{
    public class UpdateUserRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public UserType UserType { get; set; }
    }
}
