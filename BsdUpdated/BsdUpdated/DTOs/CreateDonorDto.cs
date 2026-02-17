using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BsdUpdated.DTOs
{
   
    public class CreateDonorDto
    {
        [Required,MaxLength(100)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}