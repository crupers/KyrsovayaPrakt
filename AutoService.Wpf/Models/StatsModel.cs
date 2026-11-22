namespace AutoService.Wpf.Models;

public class StatsModel
{
    public int Id { get; set; }                // ID сотрудника
    public string FullName { get; set; } = "";// ФИО
    public int TotalWorks { get; set; }        // Всего работ
    public int GoodWorksCount { get; set; }    // Хороших работ
    public int BadWorksCount { get; set; }     // Ошибок
}
