using System.ComponentModel.DataAnnotations;

namespace BsdUpdated.DTOs
{
    public class DonorDto
    {
        public int Id { get; set; }

        [Required,MaxLength(100)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}