using System.ComponentModel.DataAnnotations;

namespace BsdUpdated.DTOs
{
    public class CreateManegerDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required,MaxLength(200)]
        public string Password { get; set; }
    }
}