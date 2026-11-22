namespace AutoService.Wpf.Models;

public class WorkerModel
{
    public int Id { get; set; }                 // ID сотрудника
    public string FullName { get; set; } = ""; // ФИО
    public string Login { get; set; } = "";    // Логин
    public string Role { get; set; } = "";     // Роль
    public int GoodWorksCount { get; set; }     // Хорошие работы
    public int BadWorksCount { get; set; }      // Ошибки
    public bool IsActive { get; set; }          // Активный аккаунт
}
