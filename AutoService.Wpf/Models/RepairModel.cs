namespace AutoService.Wpf.Models;

public class RepairModel
{
    public int Id { get; set; }
    public string PlateNumber { get; set; } = "";
    public string? Brand { get; set; }
    public string? Organization { get; set; }
    public string ProblemDescription { get; set; } = "";
    public int? PreviousRepairId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = "";
    public string StatusDisplay => Status switch
    {
        "New" => "Новый",
        "Recorded" => "Работы записаны",
        "Reviewed" => "Проверен",
        _ => Status
    };
    public List<WorkModel> Works { get; set; } = new();
}
