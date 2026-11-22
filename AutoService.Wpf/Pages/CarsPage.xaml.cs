using System.Windows;
using System.Windows.Controls;
using AutoService.Wpf.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace AutoService.Wpf.Pages;

public partial class CarsPage : Page
{
    private readonly HttpClient http = new HttpClient();
    private const string api = "http://localhost:5161/";
    public CarsPage()
    {
        InitializeComponent();
        Loaded += CarsPage_Loaded;
    }

    private async void CarsPage_Loaded(object sender, RoutedEventArgs eventArgs)
    {
        await LoadCars();
    }

    private async Task LoadCars()
    {
        try
        {
            var cars = await http.GetFromJsonAsync<List<CarModel>>(
    api + "api/cars");

            CarsList.ItemsSource = cars;
        }
        catch (Exception e)
        {
            MessageText.Text = "нету соеденения" + e;
        }
    }

    private void CarsList_SelectionChanged(object sender, SelectionChangedEventArgs eventArgs)
    {
        if (CarsList.SelectedItem is not CarModel selectedCar)
            return;

        PlateBox.Text = selectedCar.PlateNumber;
        BrandBox.Text = selectedCar.Brand ?? "";
        ModelBox.Text = selectedCar.Model ?? "";
        VinBox.Text = selectedCar.Vin ?? "";
        OwnerBox.Text = selectedCar.OwnerName ?? "";
        PhoneBox.Text = selectedCar.OwnerPhone ?? "";
    }

    private async void Save_Click(object sender, RoutedEventArgs eventArgs)
    {
        if (CarsList.SelectedItem is not CarModel selectedCar)
        {
            MessageText.Text = "Выберите автомобиль";
            return;
        }

        CarModel car = new CarModel
        {
            Id = selectedCar.Id,
            PlateNumber = PlateBox.Text.Trim(),
            Brand = BrandBox.Text.Trim(),
            Model = ModelBox.Text.Trim(),
            Vin = VinBox.Text.Trim(),
            OwnerName = OwnerBox.Text.Trim(),
            OwnerPhone = PhoneBox.Text.Trim()
        };



        try
        {
            var response = await http.PutAsJsonAsync(
       api + "api/cars/" + selectedCar.Id, car);

            response.EnsureSuccessStatusCode();
            MessageText.Text = "Изменения сохранены!";
            await LoadCars();
        }
        catch (Exception error)
        {
            MessageText.Text = "Не удалось сохранить автомобиль: " + error.Message;
        }
    }
    private async void Add_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PlateBox.Text))
        {
            MessageText.Text = "Введите госномер автомобиля";
            return;
        }

        var car = new
        {
            PlateNumber = PlateBox.Text.Trim(),
            Brand = BrandBox.Text.Trim(),
            Model = ModelBox.Text.Trim(),
            Vin = VinBox.Text.Trim(),
            OwnerName = OwnerBox.Text.Trim(),
            OwnerPhone = PhoneBox.Text.Trim()
        };

        try
        {
            var response = await http.PostAsJsonAsync(
                api + "api/cars", car);

            response.EnsureSuccessStatusCode();

            MessageText.Text = "Автомобиль добавлен!";

            PlateBox.Clear();
            BrandBox.Clear();
            ModelBox.Clear();
            VinBox.Clear();
            OwnerBox.Clear();
            PhoneBox.Clear();

            await LoadCars();
        }
        catch (Exception error)
        {
            MessageText.Text = "Ошибка добавления: " + error.Message;
        }
    }
    private void Back_Click(object sender, RoutedEventArgs eventArgs)
    {
        if (NavigationService.CanGoBack)
            NavigationService.GoBack();
    }
}
