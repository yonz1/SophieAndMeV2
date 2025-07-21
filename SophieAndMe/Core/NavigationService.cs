namespace SophieAndMe.Core;

public class NavigationService
{
    
    private NavigationService() {}
    private static NavigationService _instance;
    public static NavigationService Instance => _instance ??= new NavigationService();
    public Action<object> NavigateAction { get; set; }

    public void Navigate(object view)
    {
        NavigateAction?.Invoke(view);
    }
}