using System.ComponentModel.DataAnnotations;

namespace BsdUpdated.Models
{
    public class Donor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string EMail { get; set; }
        public ICollection<Gift> GiftsList { get; set; }


    }
}
