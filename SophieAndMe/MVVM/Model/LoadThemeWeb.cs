using System.IO;
using SophieAndMe.Core;

namespace SophieAndMe.MVVM.Model;

public static class LoadThemeWeb
{
    static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\HTML_Const\ThemeMain.js");
    private static IDataService  _dataService;
    
    public static void LoadThemeWebval()
    {
        _dataService = App.DataService;
        string val = _dataService.IsDark
            ? "document.body.classList.toggle(\"Dark\");"
            : "document.body.classList.toggle(\"Light\");";
        string val2 = _dataService.IsDark
            ? "window.color = {\"Text\":\"#e2e8f0\",\"bg\":\"#242424\"}"
            : "window.color = {\"Text\":\"#1E293B\",\"bg\":\"#FFFFFF\"}";
        File.WriteAllText(path, string.Empty);
        using (StreamWriter writer = File.AppendText(path))
        {
            writer.WriteLine(val);
            writer.WriteLine(val2);
        }            
    }
}