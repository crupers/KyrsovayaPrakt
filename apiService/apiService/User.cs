using System;
using System.Collections.Generic;

namespace apiService;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public int GoodWorksCount { get; set; }

    public int BadWorksCount { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<QualityCheck> QualityChecks { get; set; } = new List<QualityCheck>();

    public virtual ICollection<RepairWork> RepairWorks { get; set; } = new List<RepairWork>();

    public virtual ICollection<Repair> Repairs { get; set; } = new List<Repair>();
}
