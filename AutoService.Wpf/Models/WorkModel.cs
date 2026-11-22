namespace AutoService.Wpf.Models;

public class WorkModel
{
    public int Id { get; set; }
    public int RepairId { get; set; }
    public string PlateNumber { get; set; } = "";
    public string WorkerName { get; set; } = "";
    public string Description { get; set; } = "";
    public string? InstalledParts { get; set; }
    public DateTime CompletedAt { get; set; }
    public string QualityStatus { get; set; } = "Pending";
    public string QualityDisplay => QualityStatus switch
    {
        "Good" => "Выполнено качественно",
        "Bad" => "Подтверждена ошибка",
        "Other" => "Нет ошибки механика",
        _ => "Ожидает проверки"
    };
}
