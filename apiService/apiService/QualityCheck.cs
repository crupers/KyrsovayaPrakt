using System;
using System.Collections.Generic;

namespace apiService;

public partial class QualityCheck
{
    public int Id { get; set; }

    public int WorkId { get; set; }

    public int CheckedByUserId { get; set; }

    public int? RepeatRepairId { get; set; }

    public string Cause { get; set; } = null!;

    public string Comment { get; set; } = null!;

    public DateTime CheckedAt { get; set; }

    public virtual User CheckedByUser { get; set; } = null!;

    public virtual Repair? RepeatRepair { get; set; }

    public virtual RepairWork Work { get; set; } = null!;
}
