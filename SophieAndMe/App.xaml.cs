using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Mapping;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SophieAndMe.Core;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;
    public static IDataService DataService { get; set; } =  new DataService();
    public static IWindowService WindowService { get; set; }


    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        // WindowService.ShowWindow<LoginWindoowViewModel>();
        WindowService = new WindowService();
        var vm = new LoginWindoowViewModel(WindowService);
        var window = new LoginWindow() {DataContext = vm};
        window.Show();
    }
    
    public App()
    {
        
    }

    
    
}