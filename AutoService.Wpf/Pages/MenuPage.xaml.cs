using System.Windows;
using System.Windows.Controls;

namespace AutoService.Wpf.Pages;

public partial class MenuPage : Page
{
    public MenuPage()
    {
        InitializeComponent();
    }

    private void Users_Click(object sender, RoutedEventArgs eventArgs)
    {
        NavigationService.Navigate(new UsersPage());
    }

    private void Cars_Click(object sender, RoutedEventArgs eventArgs)
    {
        NavigationService.Navigate(new CarsPage());
    }

    private void Repairs_Click(object sender, RoutedEventArgs eventArgs)
    {
        NavigationService.Navigate(new RepairsPage());
    }

    private void Quality_Click(object sender, RoutedEventArgs eventArgs)
    {
        NavigationService.Navigate(new QualityPage());
    }

    private void Stats_Click(object sender, RoutedEventArgs eventArgs)
    {
        NavigationService.Navigate(new StatsPage());
    }
}
