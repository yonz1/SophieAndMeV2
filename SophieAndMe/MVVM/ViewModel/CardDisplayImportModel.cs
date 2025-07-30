using System.Text.Json;
using CommunityToolkit.Mvvm.Messaging;
using SophieAndMe.Core;
using SophieAndMe.MVVM.Model;

namespace SophieAndMe.MVVM.ViewModel;

public class CardDisplayImportModel
{
    private readonly VCustomModel _vCustomModel;
    private List<string> _level;
    private List<string> _course;
    private List<string> _question;
    private List<string> _repnse;
    private List<string> _urlQuestion;
    private List<string> _urlRep;
    private List<string> _difficulty;
    public CardDisplayImportModel(VCustomModel vm)
    {
        _vCustomModel = vm;
        ShowLogic();
    }
    
    public void ShowLogic()
    {
        
        (_level, _course, _question, _urlQuestion, _repnse, _urlRep, _difficulty) = DBInteraction.GetAllPublic();
        // var jscode = WebviewInteraction.send_data_Card_Import(_level, _course,QuizzUtilities.Miseneformelist(_question),_urlQuestion,QuizzUtilities.Miseneformelist(_repnse),_urlRep,_difficulty);
        WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage("ClearCard()"));
        Dictionary<string, string> dico = new Dictionary<string, string>();
        for (int i = 0; i < _level.Count; i++)
        {
            dico["level"] = _level[i];
            dico["course"] = _course[i];
            dico["question"] = _question[i];
            dico["repnse"] = _repnse[i];
            dico["urlQuestion"] = _urlQuestion[i];
            dico["urlRep"] = _urlRep[i];
            dico["difficulty"] = _difficulty[i];
            string jscode = JsonSerializer.Serialize(dico);
            WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscode));
        }
    }
}