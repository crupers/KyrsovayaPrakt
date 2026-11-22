using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace apiService;

public partial class AutoServiceCourseContext : DbContext
{
    public AutoServiceCourseContext()
    {
    }

    public AutoServiceCourseContext(DbContextOptions<AutoServiceCourseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<QualityCheck> QualityChecks { get; set; }

    public virtual DbSet<Repair> Repairs { get; set; }

    public virtual DbSet<RepairWork> RepairWorks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=AutoServiceCourse;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cars__3214EC0700FBA3F6");

            entity.HasIndex(e => e.PlateNumber, "UQ__Cars__0369262432253B3A").IsUnique();

            entity.Property(e => e.Brand).HasMaxLength(70);
            entity.Property(e => e.Model).HasMaxLength(70);
            entity.Property(e => e.OwnerName).HasMaxLength(150);
            entity.Property(e => e.OwnerPhone).HasMaxLength(30);
            entity.Property(e => e.PlateNumber).HasMaxLength(20);
            entity.Property(e => e.Vin).HasMaxLength(17);
        });

        modelBuilder.Entity<QualityCheck>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QualityC__3214EC07E165F4DE");

            entity.HasIndex(e => e.WorkId, "IX_QualityChecks_WorkId");

            entity.Property(e => e.Cause).HasMaxLength(30);
            entity.Property(e => e.CheckedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Comment).HasMaxLength(1000);

            entity.HasOne(d => d.CheckedByUser).WithMany(p => p.QualityChecks)
                .HasForeignKey(d => d.CheckedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QualityChecks_Users");

            entity.HasOne(d => d.RepeatRepair).WithMany(p => p.QualityChecks)
                .HasForeignKey(d => d.RepeatRepairId)
                .HasConstraintName("FK_QualityChecks_Repeats");

            entity.HasOne(d => d.Work).WithMany(p => p.QualityChecks)
                .HasForeignKey(d => d.WorkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QualityChecks_Works");
        });

        modelBuilder.Entity<Repair>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Repairs__3214EC07BD1A09DC");

            entity.HasIndex(e => new { e.CarId, e.CreatedAt }, "IX_Repairs_CarId").IsDescending(false, true);

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Diagnosis).HasMaxLength(1000);
            entity.Property(e => e.Organization).HasMaxLength(150);
            entity.Property(e => e.ProblemDescription).HasMaxLength(1000);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("New");

            entity.HasOne(d => d.Car).WithMany(p => p.Repairs)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Repairs_Cars");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Repairs)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Repairs_Users");

            entity.HasOne(d => d.PreviousRepair).WithMany(p => p.InversePreviousRepair)
                .HasForeignKey(d => d.PreviousRepairId)
                .HasConstraintName("FK_Repairs_Previous");
        });

        modelBuilder.Entity<RepairWork>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RepairWo__3214EC07E203C659");

            entity.HasIndex(e => e.RepairId, "IX_RepairWorks_RepairId");

            entity.HasIndex(e => e.WorkerId, "IX_RepairWorks_WorkerId");

            entity.Property(e => e.CompletedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.InstalledParts).HasMaxLength(500);
            entity.Property(e => e.QualityStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Repair).WithMany(p => p.Works)
                .HasForeignKey(d => d.RepairId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RepairWorks_Repairs");

            entity.HasOne(d => d.Worker).WithMany(p => p.RepairWorks)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RepairWorks_Workers");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07006C8535");

            entity.HasIndex(e => e.Login, "UQ__Users__5E55825B4BF05483").IsUnique();

            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Login).HasMaxLength(60);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
