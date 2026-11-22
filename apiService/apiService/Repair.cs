using System;
using System.Collections.Generic;

namespace apiService;

public partial class Repair
{
    public int Id { get; set; }

    public int CarId { get; set; }

    public string? Organization { get; set; }

    public string ProblemDescription { get; set; } = null!;

    public string? Diagnosis { get; set; }

    public int? Mileage { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Status { get; set; } = null!;

    public int? PreviousRepairId { get; set; }

    public int CreatedByUserId { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual ICollection<Repair> InversePreviousRepair { get; set; } = new List<Repair>();

    public virtual Repair? PreviousRepair { get; set; }

    public virtual ICollection<QualityCheck> QualityChecks { get; set; } = new List<QualityCheck>();

    public virtual ICollection<RepairWork> Works { get; set; } = new List<RepairWork>();
}
