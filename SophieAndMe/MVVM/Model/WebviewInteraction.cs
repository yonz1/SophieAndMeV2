using System.Text.Json;

namespace SophieAndMe.MVVM.Model;

public abstract class WebviewInteraction
{
    public static string send_data(string action, string question, string rep, string questUrl, string repUrl)
    {
        var args = new[] { action, question, rep, questUrl.Replace("\\", "/"), repUrl.Replace("\\", "/") };
        string jsCall = $"updatequizz({JsonSerializer.Serialize(args[0])}, {JsonSerializer.Serialize(args[1])}, {JsonSerializer.Serialize(args[2])}, {JsonSerializer.Serialize(args[3])}, {JsonSerializer.Serialize(args[4])})";
        return (jsCall);
    }
    
    public static string send_data_Card_Resp(List<string> question,List<string> rep,List<string> questUrl,List<string> repUrl)
    { 
        string json = JsonSerializer.Serialize(question);
        string json2 = JsonSerializer.Serialize(rep);
        string json3 = JsonSerializer.Serialize(questUrl);
        string json4 = JsonSerializer.Serialize(repUrl);
        string jsCode = $"CreateCardResp({json},{json2},{json3},{json4})";
        return jsCode;
    }
    
    public static string send_data_Card_Marked(List<string> question,List<string> rep,List<string> questUrl,List<string> repUrl)
    { 
        string json = JsonSerializer.Serialize(question);
        string json2 = JsonSerializer.Serialize(rep);
        string json3 = JsonSerializer.Serialize(questUrl);
        string json4 = JsonSerializer.Serialize(repUrl);
        string jsCode = $"CreateCardMarked({json},{json2},{json3},{json4})";
        return jsCode;
    }
    
    
    public static string send_data_Card_Created(List<string> question,List<string> rep,List<string> questUrl,List<string> repUrl)
    { 
        string json = JsonSerializer.Serialize(question);
        string json2 = JsonSerializer.Serialize(rep);
        string json3 = JsonSerializer.Serialize(questUrl);
        string json4 = JsonSerializer.Serialize(repUrl);
        string jsCode = $"CreateCardCreated({json},{json2},{json3},{json4})";
        return jsCode;
    }

    public static string Initcustom(List<string> nomsSugg, string action)
    {
        string json = JsonSerializer.Serialize(nomsSugg);
        string json2 = JsonSerializer.Serialize(action);
        string jscode = $"Suggestion({json},{json2})";
        return jscode;
    }

    public static string EdtiQuizz(string matier, string name, string question,string imgQuestion,  string rep, string imgRep)
    {
        string json = JsonSerializer.Serialize(matier);
        string json2 = JsonSerializer.Serialize(name);
        string json3 = JsonSerializer.Serialize(question);
        string json4 = JsonSerializer.Serialize(imgQuestion);
        string json5 = JsonSerializer.Serialize(rep);
        string json6 = JsonSerializer.Serialize(imgRep);
        string jscode = $"fill_edit({json},{json2},{json3},{json4},{json5},{json6})";
        return jscode;
    }

    public static string send_data_Card_Import(List<string> level, List<string> course, List<string> question, List<string> imgQuestion,
        List<string> rep, List<string> imgRep, List<string> difficulty)
    {
        string json = JsonSerializer.Serialize(level);
        string json2 = JsonSerializer.Serialize(course);
        string json3 = JsonSerializer.Serialize(question);
        string json4 = JsonSerializer.Serialize(imgQuestion);
        string json5 = JsonSerializer.Serialize(rep);
        string json6 = JsonSerializer.Serialize(imgRep);
        string json7 = JsonSerializer.Serialize(difficulty);
        string jscode = $"CreateCardResp({json3},{json5},{json4},{json6})";
        return jscode;
    }
    
    public static List<string> LandingNotes(List<string> meta,List<string> data)
    {
        string val = "";
        List<string> JsCode = [];
        Console.WriteLine("Landingnotes appelé");
        Console.WriteLine(meta.Count);
        for (int i = 0; i < meta.Count; i++)
        {
            val = $"FillMessages({JsonSerializer.Serialize(meta[i])},{JsonSerializer.Serialize(data[i])},{JsonSerializer.Serialize("Notes")})";
            JsCode.Add(val);
        }
        return JsCode;
    }

    public static List<string> LandingQuizz(List<string> name)
    {
        string info = "";
        string json = "";
        List<string> JsCode = [];
        foreach (var val in name)
        {
            info = $"FillMessages({JsonSerializer.Serialize(DbInteraction.GetMat(val))},{JsonSerializer.Serialize(val)},{JsonSerializer.Serialize("Quizz")})";
            JsCode.Add(json);
        }

        return JsCode;
    }
    
    
    

}