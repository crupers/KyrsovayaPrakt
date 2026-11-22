using apiService.Context;
using Microsoft.AspNetCore.Mvc;
using apiService;
using apiService.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiService.Controllers
{
    [ApiController]
    [Route("api/repairs")]
    public class RepairsController : Controller
    {
        private readonly AutoServiceContext _db;

        public RepairsController(AutoServiceContext db)
        {
            _db = db;
        }

        // Получение истории ремонтов
        [HttpGet]
        public async Task<IActionResult> GetAll(string? plate)
        {
            var repairs = await _db.Repairs
                .AsNoTracking()
                .Where(r => plate == null ||
                    _db.Cars.Any(c =>
                        c.Id == r.CarId &&
                        c.PlateNumber.Contains(plate)))
                .Select(r => new
                {
                    r.Id,
                    PlateNumber = _db.Cars
                        .Where(c => c.Id == r.CarId)
                        .Select(c => c.PlateNumber)
                        .FirstOrDefault(),

                    r.Organization,
                    r.ProblemDescription,
                    r.Diagnosis,
                    r.Mileage,
                    r.CreatedAt,
                    r.Status,
                    r.PreviousRepairId,
                    r.CreatedByUserId,

                    Works = _db.RepairWorks
                        .Where(w => w.RepairId == r.Id)
                        .Select(w => new
                        {
                            w.Id,
                            w.RepairId,
                            w.Description,
                            w.InstalledParts,
                            w.CompletedAt,
                            w.QualityStatus,
                            WorkerName = _db.Users
                                .Where(u => u.Id == w.WorkerId)
                                .Select(u => u.FullName)
                                .FirstOrDefault()
                        }).ToList()
                })
                .ToListAsync();

            return Ok(repairs);
        }

     

        // Создание ремонта
        [HttpPost]
        public async Task<IActionResult> Add(
            CreateRepairRequest request)
        {
            if (!await _db.Cars.AnyAsync(
                c => c.Id == request.CarId))
                return BadRequest("Автомобиль не найден.");

            if (!await _db.Users.AnyAsync(
                u => u.Id == request.CreatedByUserId))
                return BadRequest("Сотрудник не найден.");

            if (string.IsNullOrWhiteSpace(request.ProblemDescription))
                return BadRequest("Укажите неисправность.");

            var repair = new Repair
            {
                CarId = request.CarId,
                CreatedByUserId = request.CreatedByUserId,
                Organization = request.Organization,
                ProblemDescription = request.ProblemDescription,
                Diagnosis = request.Diagnosis,
                Mileage = request.Mileage ?? 0,
                PreviousRepairId = request.PreviousRepairId,
                CreatedAt = DateTime.Now,
                Status = "New"
            };

            _db.Repairs.Add(repair);
            await _db.SaveChangesAsync();

            return Ok(new { repair.Id });
        }

        public class CreateWorkRequest
        {
            public int WorkerId { get; set; }
            public string Description { get; set; } = "";
            public string? InstalledParts { get; set; }
        }

        // Добавление выполненной работы
        [HttpPost("{id}/works")]
        public async Task<IActionResult> AddWork(
            int id, CreateWorkRequest request)
        {
            var repair = await _db.Repairs.FindAsync(id);

            if (repair == null)
                return NotFound("Ремонт не найден.");

            if (!await _db.Users.AnyAsync(
                u => u.Id == request.WorkerId &&
                     u.Role == "Worker"))
                return BadRequest("Механик не найден.");

            var work = new RepairWork
            {
                RepairId = id,
                WorkerId = request.WorkerId,
                Description = request.Description,
                InstalledParts = request.InstalledParts,
                CompletedAt = DateTime.Now,
                QualityStatus = "Pending"
            };

            _db.RepairWorks.Add(work);
            repair.Status = "Recorded";

            await _db.SaveChangesAsync();

            return Ok(new { work.Id });
        }
    }
}