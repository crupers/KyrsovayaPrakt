using Microsoft.AspNetCore.Mvc;

using apiService.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace apiService.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : Controller
    {
        private readonly AutoServiceContext _db;

        public StatisticsController(AutoServiceContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            DateTime? from, DateTime? to)
        {
            var statistics = await _db.Users
                .Where(u => u.Role == "Worker")
                .Select(u => new
                {
                    u.Id,
                    u.FullName,

                    TotalWorks = _db.RepairWorks.Count(w =>
                        w.WorkerId == u.Id &&
                        (!from.HasValue ||
                         w.CompletedAt >= from.Value) &&
                        (!to.HasValue ||
                         w.CompletedAt < to.Value)),

                    GoodWorksCount = _db.RepairWorks.Count(w =>
                        w.WorkerId == u.Id &&
                        w.QualityStatus == "Good" &&
                        (!from.HasValue ||
                         w.CompletedAt >= from.Value) &&
                        (!to.HasValue ||
                         w.CompletedAt < to.Value)),

                    BadWorksCount = _db.RepairWorks.Count(w =>
                        w.WorkerId == u.Id &&
                        w.QualityStatus == "Bad" &&
                        (!from.HasValue ||
                         w.CompletedAt >= from.Value) &&
                        (!to.HasValue ||
                         w.CompletedAt < to.Value))
                })
                .ToListAsync();

            return Ok(statistics);
        }
    }
}