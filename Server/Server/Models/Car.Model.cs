namespace Server.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string ChasisNumber { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleColor { get; set; }
        public string VehicleColorHex { get; set; }
        public string Status { get; set; } // "Sold", "Unsold", "Maintenance"
        public DateTime DateIn { get; set; }
        public DateTime? DateOut { get; set; }
        public string Branch { get; set; }

        public int DealerId { get; set; }
        public required User Dealer { get; set; }
    }
}