using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Mapping;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SophieAndMe.Core;

namespace SophieAndMe;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;
    public static IDataService DataService { get; set; } =  new DataService();
    public App()
    {
        
    }

    
    
}