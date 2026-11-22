using apiService.Context;
using Microsoft.AspNetCore.Mvc;

namespace apiService.Controllers
{
    using apiService;
    using apiService.Context;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    [ApiController]
    [Route("api/quality")]
    public class QualityController : Controller
    {
        private readonly AutoServiceContext _db;

        public QualityController(AutoServiceContext db)
        {
            _db = db;
        }

        // Работы, ожидающие проверки
        [HttpGet("works")]
        public async Task<IActionResult> GetWorks()
        {
            var works = await _db.RepairWorks
                .Where(w => w.QualityStatus == "Pending")
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
                        .FirstOrDefault(),

                    PlateNumber = _db.Repairs
                        .Where(r => r.Id == w.RepairId)
                        .Join(_db.Cars,
                            r => r.CarId,
                            c => c.Id,
                            (r, c) => c.PlateNumber)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(works);
        }

        public class CheckRequest
        {
            public int CheckedByUserId { get; set; }
            public string Cause { get; set; } = "";
            public string Comment { get; set; } = "";
            public int? RepeatRepairId { get; set; }
        }

        // Проверка качества
        [HttpPost("{workId}/check")]
        public async Task<IActionResult> Check(
            int workId, CheckRequest request)
        {
            var work = await _db.RepairWorks.FindAsync(workId);

            if (work == null)
                return NotFound("Работа не найдена.");

            if (await _db.QualityChecks.AnyAsync(
                q => q.WorkId == workId))
                return BadRequest("Работа уже проверена.");

            if (request.Cause != "Good" &&
                request.Cause != "Bad" &&
                request.Cause != "Other")
                return BadRequest("Неверный результат проверки.");

            if (!await _db.Users.AnyAsync(
                u => u.Id == request.CheckedByUserId &&
                     u.Role == "Admin"))
                return BadRequest("Администратор не найден.");

            var check = new QualityCheck
            {
                WorkId = workId,
                CheckedByUserId = request.CheckedByUserId,
                RepeatRepairId = request.RepeatRepairId,
                Cause = request.Cause,
                Comment = request.Comment,
                CheckedAt = DateTime.Now
            };

            _db.QualityChecks.Add(check);

            work.QualityStatus = request.Cause;

            var repair = await _db.Repairs.FindAsync(work.RepairId);

            if (repair != null)
                repair.Status = "Reviewed";

            await _db.SaveChangesAsync();

            return Ok(new { message = "Проверка сохранена" });
        }
    }
}