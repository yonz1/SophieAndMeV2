namespace SophieAndMe.Core;

public class NavigationService
{
    
    private static NavigationService _instance;
    public static NavigationService Instance => _instance ??= new NavigationService();

    private readonly Dictionary<string, Action<object>> _navigateActions = new();

    // Permet d’enregistrer une "zone" de navigation
    public void Register(string key, Action<object> navigateAction)
    {
        if (!_navigateActions.ContainsKey(key))
            _navigateActions.Add(key, navigateAction);
        else
            _navigateActions[key] = navigateAction;
    }

    // Permet de naviguer dans une zone donnée
    public void Navigate(string key, object view)
    {
        if (_navigateActions.TryGetValue(key, out var action))
            action.Invoke(view);
    }
}