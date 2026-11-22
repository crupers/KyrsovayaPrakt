namespace AutoService.Wpf.Models;

public class CarModel
{
    public int Id { get; set; }                   // ID автомобиля
    public string PlateNumber { get; set; } = "";// Госномер
    public string? Brand { get; set; }            // Марка
    public string? Model { get; set; }            // Модель
    public string? Vin { get; set; }             
    public string? OwnerName { get; set; }        // Владелец
    public string? OwnerPhone { get; set; }       // Контакт
}
