using System.Windows;
using System.Windows.Controls;
using AutoService.Wpf.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace AutoService.Wpf.Pages;

public partial class RepairsPage : Page
{
    private readonly HttpClient http = new HttpClient();
    private const string api = "http://localhost:5161/";
    public RepairsPage()
    {
        InitializeComponent();
        Loaded += RepairsPage_Loaded;
    }

    private async void RepairsPage_Loaded(object sender, RoutedEventArgs eventArgs)
    {
        await LoadRepairs("");
    }

    private async Task LoadRepairs(string plateNumber)
    {
        try
        {
            string address = api + "api/repairs";

            if (!string.IsNullOrWhiteSpace(plateNumber))
            {
                address += "?plate=" + Uri.EscapeDataString(plateNumber);
            }

            var repairs = await http.GetFromJsonAsync<List<RepairModel>>(
                address);

            RepairsList.ItemsSource = repairs;
            WorksList.ItemsSource = null;
        }
        catch (Exception e)
        {
            MessageBox.Show("Не удалось загрузить ремонты: " + e.Message);
        }
    }

    private async void Search_Click(object sender, RoutedEventArgs eventArgs)
    {
        await LoadRepairs(PlateFilter.Text.Trim());
    }

    private async void All_Click(object sender, RoutedEventArgs eventArgs)
    {
        PlateFilter.Clear();
        await LoadRepairs("");
    }

    private void RepairsList_SelectionChanged(object sender, SelectionChangedEventArgs eventArgs)
    {
        RepairModel? selectedRepair = RepairsList.SelectedItem as RepairModel;
        WorksList.ItemsSource = selectedRepair?.Works;
    }

    private void Back_Click(object sender, RoutedEventArgs eventArgs)
    {
        if (NavigationService.CanGoBack)
            NavigationService.GoBack();
    }
}
