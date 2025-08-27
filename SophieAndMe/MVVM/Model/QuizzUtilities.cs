
using System.Text.RegularExpressions;

namespace SophieAndMe.MVVM.Model;

public class QuizzUtilities
{
    // ################################################### Initialisations 
    
    private static readonly List<string> Question = new List<string>();
    static readonly List<string> Repnse = new List<string>();
    private static readonly List<string> UrlQuestion = new List<string>();
    private static readonly List<string> UrlRep = new List<string>();
    
    // ################################################### Fonctions
    
    public static (List<string> Question,List<string> Response,List<string> Url_QUestion, List<string> Url_Response) Shuffle( List<string>  aquestion, List<string>  arepnse, List<string>  aurlQuestion, List<string> aurlRep)
    {
        Question.Clear();
        Repnse.Clear();
        UrlQuestion.Clear();
        UrlRep.Clear();
        var random = new Random();
        var indices = new List<int>();
        for (int i = 0 ; i < aquestion.Count ; i++)
        {
            indices.Add(i);
        }
        for (int i = indices.Count - 1; i > 0; i--)
        { 
            int j = random.Next(i+1);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }
        foreach (int i in indices)
        {
            System.Diagnostics.Debug.WriteLine("################################################ Premiére serie d'ajout : ", i.ToString());
            Question.Add(aquestion[i]);
            Repnse.Add(arepnse[i]);
            UrlQuestion.Add(aurlQuestion[i]);
            UrlRep.Add(aurlRep[i]);
        }
        return (Question,Repnse,UrlQuestion,UrlRep);
    }
    
    public static string Miseneformetext(string text)
    {
        text = text.Replace("\\/", "/").Replace("<", "\\lt ").Replace(">", "\\gt ").Replace("\"","'");
        return text;
    }
    
    public static List<string> Miseneformelist(List<string> list)
    {
        List<string> result =  new List<string>();
        foreach (string text in list)
        {
            result.Add(Miseneformetext(text));
        }
        return result;
    }
    
    
    
    

}