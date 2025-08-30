using SophieAndMe.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FontAwesome.Sharp;
using SophieAndMe.MVVM.View;
using SophieAndMe.Core;
using SophieAndMe.MVVM.Model;
using SophieAndMe.MVVM.View.SettingsViews;

namespace SophieAndMe.MVVM.ViewModel;

public class SettingsViewModel : ObservableRecipient, INotifyPropertyChanged

{
    public SettingsViewModel(MainViewModel viewModel)
    {
        NavigationService.Instance.Navigate("SettingsMain", new SettingsProfile(viewModel));
    }

}