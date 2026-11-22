using apiService.Context;
using Microsoft.AspNetCore.Mvc;
using apiService;
using Microsoft.EntityFrameworkCore;
namespace apiService.Controllers
{
    [ApiController]
    [Route("api/cars")]
    public class CarsController : Controller
    {
        private readonly AutoServiceContext _db;

        public CarsController(AutoServiceContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cars = await _db.Cars.ToListAsync();
            return Ok(cars);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Car car)
        {
            if (string.IsNullOrWhiteSpace(car.PlateNumber))
                return BadRequest("Укажите госномер.");

            if (await _db.Cars.AnyAsync(
                x => x.PlateNumber == car.PlateNumber))
                return BadRequest("Автомобиль уже существует.");

            car.Id = 0;

            _db.Cars.Add(car);
            await _db.SaveChangesAsync();

            return Ok(car);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Car data)
        {
            var car = await _db.Cars.FindAsync(id);

            if (car == null)
                return NotFound();

            car.PlateNumber = data.PlateNumber;
            car.Brand = data.Brand;
            car.Model = data.Model;
            car.Vin = data.Vin;
            car.OwnerName = data.OwnerName;
            car.OwnerPhone = data.OwnerPhone;

            await _db.SaveChangesAsync();

            return Ok(car);
        }
    }
}



