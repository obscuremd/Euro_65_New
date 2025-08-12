using System.ComponentModel.DataAnnotations;

namespace Server.DTO
{
    public class CarDto
    {
        [Required]
        public string ChasisNumber { get; set; }

        [Required]
        public string VehicleBrand { get; set; }

        [Required]
        public string VehicleColor { get; set; }

        [Required]
        public string VehicleColorHex { get; set; }

        [Required]
        [RegularExpression("Sold|Unsold|Maintenance", ErrorMessage = "Status must be Sold, Unsold, or Maintenance.")]
        public string Status { get; set; }

        [Required]
        public DateTime DateIn { get; set; }

        public DateTime? DateOut { get; set; }

        [Required]
        public string Branch { get; set; }

        [Required(ErrorMessage = "DealerId is required")]
        public int DealerId { get; set; }
    }
}
