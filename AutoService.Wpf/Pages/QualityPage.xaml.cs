using System.Windows;
using System.Windows.Controls;
using AutoService.Wpf.Models;
using System.Net.Http;
using System.Net.Http.Json;


namespace AutoService.Wpf.Pages;

public partial class QualityPage : Page
{
    private readonly HttpClient http = new HttpClient();
    private const string api = "http://localhost:5161/";
    private readonly int adminId = 1;
    public QualityPage()
    {
        InitializeComponent();
        Loaded += QualityPage_Loaded;
    }

    private async void QualityPage_Loaded(object sender, RoutedEventArgs eventArgs)
    {
        await LoadWorks();
    }

    private async Task LoadWorks()
    {
        try
        {
            var works = await http.GetFromJsonAsync<List<WorkModel>>(
       api + "api/quality/works");

            PendingList.ItemsSource = works;
        }
        catch (Exception e)
        {
            MessageText.Text = "Не удалось загрузить работы: " + e.Message;
        }
    }

    private async void Check_Click(object sender, RoutedEventArgs eventArgs)
    {
        if (PendingList.SelectedItem is not WorkModel selectedWork)
        {
            MessageText.Text = "Выберите работу для проверки";
            return;
        }

        int? repeatRepairId = null;
        string repeatText = RepeatBox.Text.Trim();
        if (repeatText.Length > 0)
        {
            if (!int.TryParse(repeatText, out int repairId))
            {
                MessageText.Text = "ID повторного ремонта должен быть числом";
                return;
            }
            repeatRepairId = repairId;
        }

        string cause = (CauseBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Good";
        string comment = CommentBox.Text.Trim();
        if (comment.Length == 0)
        {
            MessageText.Text = "Напишите причину решения";
            return;
        }

        try
        {
            var check = new
            {
                CheckedByUserId = adminId,
                Cause = cause,
                Comment = comment,
                RepeatRepairId = repeatRepairId
            };

            var response = await http.PostAsJsonAsync(
                api + "api/quality/" + selectedWork.Id + "/check",
                check);

            response.EnsureSuccessStatusCode();
            CommentBox.Clear();
            RepeatBox.Clear();
            MessageText.Text = "Проверка сохранена";
            await LoadWorks();
        }
        catch (Exception error)
        {
            MessageText.Text = "Не удалось сохранить проверку: " + error.Message;
        }
    }

    private void Back_Click(object sender, RoutedEventArgs eventArgs)
    {
        if (NavigationService.CanGoBack)
            NavigationService.GoBack();
    }
}
