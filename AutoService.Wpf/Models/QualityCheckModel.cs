namespace AutoService.Wpf.Models;

public class QualityCheckModel
{
    public int WorkId { get; set; }
    public string Cause { get; set; } = "";
    public string Comment { get; set; } = "";
    public int? RepeatRepairId { get; set; }
    public DateTime CheckedAt { get; set; }
}
