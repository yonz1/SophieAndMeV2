using System.Windows;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.Core;
public interface IWindowService
{
    void ShowWindow<TViewModel>(object? parameter = null);
    void CloseWindow<TViewModel>();
}

public class WindowService : IWindowService
{
    private readonly Dictionary<Type, Type> _windowMap = new();

    public WindowService()
    {
        _windowMap[typeof(LoginWindoowViewModel)] = typeof(LoginWindow);
        _windowMap[typeof(MainViewModel)] = typeof(MainWindow);
    }

    public void ShowWindow<TViewModel>(object? parameter = null)
    {
        if (_windowMap.TryGetValue(typeof(TViewModel), out var windowType))
        {
            var window = (Window)Activator.CreateInstance(windowType)!;
            window.DataContext = Activator.CreateInstance(typeof(TViewModel), parameter);
            window.Show();
        }
    }

    public void CloseWindow<TViewModel>()
    {
        foreach (Window win in Application.Current.Windows)
        {
            if (win.DataContext?.GetType() == typeof(TViewModel))
            {
                win.Close();
                break;
            }
        }
    }
}

