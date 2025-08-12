using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Controllers;
using Server.DTO;
using Server.Models;

namespace Server
{
    [ApiController]
    [Route("api/{controller}")]
    public class CarController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("/create-car/{Id}")]
        public async Task<IActionResult> CreateCar(int Id, [FromBody] CarDto car)
        {
            var user = await _context.Users.FindAsync(Id);
            var dealer = await _context.Users.FindAsync(car.DealerId);
            if (user == null)
                return BadRequest(new { message = "user not found" });

            if (dealer == null)
                return BadRequest(new { message = "dealer not found" });

            if(dealer.Role != "Dealer")

            if (user.Role != "Secretary")
                return BadRequest(new { message = "only secretaries can create cars" });



            var data = new Car
            {
                ChasisNumber = car.ChasisNumber,
                VehicleBrand = car.VehicleBrand,
                VehicleColor = car.VehicleColor,
                VehicleColorHex = car.VehicleColorHex,
                Status = car.Status,
                DateIn = car.DateIn,
                DateOut = car.DateOut,
                Branch = car.Branch,
                DealerId = car.DealerId,
                Dealer = dealer
            };

            _context.Cars.Add(data);
            await _context.SaveChangesAsync();
            return Ok(new { message = "car created" });
        }
    }
}