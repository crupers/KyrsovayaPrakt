using AutoService.Wpf.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;

namespace AutoService.Wpf.Pages;

public partial class StatsPage : Page
{
    private readonly HttpClient http = new HttpClient();
    private const string api = "http://localhost:5161/";
    public StatsPage()
    {
        InitializeComponent();
        Loaded += StatsPage_Loaded;
    }

    private async void StatsPage_Loaded(object sender, RoutedEventArgs eventArgs)
    {
        await LoadStatistics();
    }

    private async Task LoadStatistics()
    {
        try
        {
            string address = api + "api/statistics";

            List<string> dates = new List<string>();

            if (FromPicker.SelectedDate.HasValue)
            {
                dates.Add("from=" +
                    FromPicker.SelectedDate.Value.ToString("yyyy-MM-dd"));
            }

            if (ToPicker.SelectedDate.HasValue)
            {
                dates.Add("to=" +
                    ToPicker.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd"));
            }

            if (dates.Count > 0)
            {
                address += "?" + string.Join("&", dates);
            }

            var statistics = await http.GetFromJsonAsync<List<StatsModel>>(
                address);

            StatsList.ItemsSource = statistics;
        }
        catch (Exception error)
        {
            MessageBox.Show("Не удалось загрузить статистику: " + error.Message);
        }
    }

    private async void Reload_Click(object sender, RoutedEventArgs eventArgs)
    {
        if (FromPicker.SelectedDate > ToPicker.SelectedDate && ToPicker.SelectedDate.HasValue)
        {
            MessageBox.Show("Дата начала позже даты окончания");
            return;
        }
        await LoadStatistics();
    }

    private async void All_Click(object sender, RoutedEventArgs eventArgs)
    {
        FromPicker.SelectedDate = null;
        ToPicker.SelectedDate = null;
        await LoadStatistics();
    }

    private void Back_Click(object sender, RoutedEventArgs eventArgs)
    {
        if (NavigationService.CanGoBack)
            NavigationService.GoBack();
    }
}
