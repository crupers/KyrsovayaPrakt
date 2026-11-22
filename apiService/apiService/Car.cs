using System;
using System.Collections.Generic;

namespace apiService;

public partial class Car
{
    public int Id { get; set; }

    public string PlateNumber { get; set; } = null!;

    public string? Brand { get; set; }

    public string? Model { get; set; }

    public string? Vin { get; set; }

    public string? OwnerName { get; set; }

    public string? OwnerPhone { get; set; }

    public virtual ICollection<Repair> Repairs { get; set; } = new List<Repair>();
}
