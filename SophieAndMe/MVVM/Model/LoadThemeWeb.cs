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
        string val = $"document.body.classList.toggle(\"{_dataService.MainThemeColor}\")";
        // string val2 = _dataService.IsDark
        //     ? "window.color = {\"Text\":\"#e2e8f0\",\"bg\":\"#334155\"}"
        //     : "window.color = {\"Text\":\"#1E293B\",\"bg\":\"#FFFFFF\"}";
        string val2 = "";
        switch (_dataService.MainThemeColor)
        {
            case "Blue":
                val2 = "window.color = {\"Text\":\"#e2e8f0\",\"bg\":\"#334155\"}";
                break;
            case "Dark":
                val2 = "window.color = {\"Text\":\"#e2e8f0\",\"bg\":\"#242424\"}";
                break;
            case "Light":
                val2 = "window.color = {\"Text\":\"#1E293B\",\"bg\":\"#FFFFFF\"}";
                break;
        }
        File.WriteAllText(path, string.Empty);
        using (StreamWriter writer = File.AppendText(path))
        {
            writer.WriteLine(val);
            writer.WriteLine(val2);
        }            
    }
}