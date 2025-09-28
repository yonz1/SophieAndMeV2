using System.Net.Http;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack; 
using System.Globalization; 


namespace SophieAndMe.MVVM.Model;
public class WebInteraction
{

    public static string GetProgKholle(string date)
    {
        string url = "https://solnon.fr/maths_kholles.html";
        var web = new HtmlWeb();
        string text =  web.Load(url).DocumentNode.InnerHtml;
        List<string> Temp = new List<string>();
        List<string> Final = new List<string>();
        string start = "<li>";
        string end = "</li>";
        string pattern = $"{Regex.Escape(start)}(.*?){Regex.Escape(end)}";
        MatchCollection matches = Regex.Matches(text, pattern);
        foreach (Match match in matches)
        {
            if (match.Groups[1].Value.Contains(date))
            {
                start = "href=\"";
                end = "\"";
                pattern = $"{Regex.Escape(start)}(.*?){Regex.Escape(end)}";
                matches = Regex.Matches(match.Groups[1].Value, pattern);
                Console.WriteLine(matches[0].Groups[1].Value);
                return ("https://solnon.fr/" +matches[0].Groups[1].Value);    
            }
        }
        return "";
    }

    public static async Task<string> GetWebPage()
    {
        using (var client = new HttpClient())
        {
            try
            {
                string url = "https://solnon.fr/maths_kholles.html";
                string content = await client.GetStringAsync(url);
                return content;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    
}