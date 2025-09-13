using System.Windows.Controls;
using SophieAndMe.Core;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View;

public partial class Settings : UserControl
{
    
    public Settings(MainViewModel viewModel)
    {
        InitializeComponent();
        NavigationService.Instance.Register("SettingsMain", viewSettings => SettingsContentControl.Content = viewSettings);
        this.DataContext = new SettingsViewModel(viewModel);
    }
}