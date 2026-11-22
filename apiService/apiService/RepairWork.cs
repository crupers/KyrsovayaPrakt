using System;
using System.Collections.Generic;

namespace apiService;

public partial class RepairWork
{
    public int Id { get; set; }

    public int RepairId { get; set; }

    public int WorkerId { get; set; }

    public string Description { get; set; } = null!;

    public string? InstalledParts { get; set; }

    public DateTime CompletedAt { get; set; }

    public string QualityStatus { get; set; } = null!;

    public virtual ICollection<QualityCheck> QualityChecks { get; set; } = new List<QualityCheck>();

    public virtual Repair Repair { get; set; } = null!;

    public virtual User Worker { get; set; } = null!;
}
