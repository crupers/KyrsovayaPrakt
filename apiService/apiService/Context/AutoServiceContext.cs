using Microsoft.EntityFrameworkCore;
namespace apiService.Context
{
    public class AutoServiceContext : DbContext
    {
        public AutoServiceContext(
      DbContextOptions<AutoServiceContext> options
  ) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<RepairWork> RepairWorks { get; set; }
        public DbSet<QualityCheck> QualityChecks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Пользователи
            modelBuilder.Entity<User>()
                .ToTable("Users");

            // Автомобили
            modelBuilder.Entity<Car>()
                .ToTable("Cars");

            // Ремонты
            modelBuilder.Entity<Repair>()
                .ToTable("Repairs");

            modelBuilder.Entity<Repair>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Repairs)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Repair>()
                .HasOne(r => r.CreatedByUser)
                .WithMany()
                .HasForeignKey(r => r.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Repair>()
                .HasOne<Repair>()
                .WithMany()
                .HasForeignKey(r => r.PreviousRepairId)
                .OnDelete(DeleteBehavior.NoAction);

            // Выполненные работы
            modelBuilder.Entity<RepairWork>()
                .ToTable("RepairWorks");

            modelBuilder.Entity<RepairWork>()
                .HasOne(w => w.Repair)
                .WithMany(r => r.Works)
                .HasForeignKey(w => w.RepairId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RepairWork>()
                .HasOne(w => w.Worker)
                .WithMany(u => u.RepairWorks)
                .HasForeignKey(w => w.WorkerId)
                .OnDelete(DeleteBehavior.NoAction);

            // Проверка качества
            modelBuilder.Entity<QualityCheck>()
                .ToTable("QualityChecks");

            modelBuilder.Entity<QualityCheck>()
                .HasOne(q => q.Work)
                .WithMany(w => w.QualityChecks)
                .HasForeignKey(q => q.WorkId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<QualityCheck>()
                .HasOne(q => q.CheckedByUser)
                .WithMany()
                .HasForeignKey(q => q.CheckedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<QualityCheck>()
                .HasOne(q => q.RepeatRepair)
                .WithMany()
                .HasForeignKey(q => q.RepeatRepairId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}