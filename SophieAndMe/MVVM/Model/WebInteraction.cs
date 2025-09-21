using System.Net.Http;
using System.Net.Http;
using System.Text.RegularExpressions;


namespace SophieAndMe.MVVM.Model;
public class WebInteraction
{

    private static void RetrieveSemKholle(string text,string date)
    {
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
            }
        }
    }
    public static async Task GetProgKholle()
    {
        using (var client = new HttpClient())
        {
            try
            {
                string url = "https://solnon.fr/maths_kholles.html";
                string content = await client.GetStringAsync(url);
                RetrieveSemKholle(content,"8 septembre");
                // Console.WriteLine(content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    
}