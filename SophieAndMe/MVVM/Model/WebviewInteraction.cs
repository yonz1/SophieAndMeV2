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
    

}