using System.Windows;
using AutoService.Wpf.Pages;

namespace AutoService.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs eventArgs)
    {
        MainFrame.NavigationService.Navigate(new MenuPage());
        Loaded -= MainWindow_Loaded;
    }
}
