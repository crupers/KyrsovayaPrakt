using AutoService.Wpf.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;

namespace AutoService.Wpf.Pages;

public partial class UsersPage : Page
{
    private readonly HttpClient http = new HttpClient();
    private const string api = "http://localhost:5161/";
    public UsersPage()
    {
        InitializeComponent();
        Loaded += UsersPage_Loaded;
    }

    private async void UsersPage_Loaded(object sender, RoutedEventArgs eventArgs)
    {
        await LoadWorkers();
    }

    private async Task LoadWorkers()
    {
        try
        {
            var workers = await http.GetFromJsonAsync<List<WorkerModel>>(
     api + "api/users");

            UsersList.ItemsSource = workers;
        }
        catch (Exception e)
        {
            MessageText.Text = "нету соеденения " +e ;
        }
    }

    private async void Create_Click(object sender, RoutedEventArgs eventArgs)
    {
        string fullName = FullNameBox.Text.Trim();
        string login = LoginBox.Text.Trim();
        string password = PasswordBox.Password;
        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(login) || password.Length < 8)
        {
            MessageText.Text = "Введите ФИО, логин и пароль не короче 8 символов.";
            return;
        }

        try
        {
            var worker = new
            {
                FullName = fullName,
                Login = login,
                Password = password
            };

            var response = await http.PostAsJsonAsync(
                api + "api/users", worker);

            response.EnsureSuccessStatusCode();
            FullNameBox.Clear();
            LoginBox.Clear();
            PasswordBox.Clear();
            MessageText.Text = "Механик добавлен.";
            await LoadWorkers();
        }
        catch (Exception e)
        {
            MessageText.Text = "Не удалось добавить механика: " + e.Message;
        }
    }

    private void Back_Click(object sender, RoutedEventArgs eventArgs)
    {
        if (NavigationService.CanGoBack)
            NavigationService.GoBack();
    }
}
