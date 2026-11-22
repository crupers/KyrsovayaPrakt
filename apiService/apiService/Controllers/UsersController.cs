using apiService.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace apiService.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly AutoServiceContext _db;

        public UsersController(AutoServiceContext db)
        {
            _db = db;
        }

        // Получить список сотрудников
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var workers = await _db.Users
                .Where(x => x.Role == "Worker")
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    x.Login,
                    x.Role,
                    x.IsActive,

                    GoodWorksCount = x.RepairWorks.Count(w =>
                        w.QualityChecks.Any(q => q.Cause == "Good")),

                    BadWorksCount = x.RepairWorks.Count(w =>
                        w.QualityChecks.Any(q => q.Cause == "Bad"))
                })
                .ToListAsync();

            return Ok(workers);
        }


        // Добавить механика
        [HttpPost]
        public async Task<IActionResult> Add(CreateWorkerRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName) ||
                string.IsNullOrWhiteSpace(request.Login) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                request.Password.Length < 8)
            {
                return BadRequest(
                    "Нужны ФИО, логин и пароль от 8 символов.");
            }

            if (await _db.Users.AnyAsync(
                x => x.Login == request.Login))
            {
                return Conflict("Такой логин уже существует.");
            }

            byte[] salt = RandomNumberGenerator.GetBytes(16);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                request.Password,
                salt,
                100_000,
                HashAlgorithmName.SHA256,
                32
            );

            var worker = new User
            {
                FullName = request.FullName.Trim(),
                Login = request.Login.Trim(),
                Role = "Worker",
                IsActive = true,
                GoodWorksCount = 0,
                BadWorksCount = 0,
                PasswordHash =
                    Convert.ToBase64String(salt) + ":" +
                    Convert.ToBase64String(hash)
            };

            _db.Users.Add(worker);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                worker.Id,
                worker.FullName
            });
        }
    }
}