using System.ComponentModel.DataAnnotations;

namespace Talabat.APIS.DTOS
{
    public class AddressDto
    {
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string street { get; set; }
    }
}
