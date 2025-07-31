using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
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

    

    public CardDisplayImportModel(VCustomModel vm,string action)
    {
        _vCustomModel = vm;
        
        
        WeakReferenceMessenger.Default.Register<MediatorCustom.JstoAppMessage>(this, (r, m) =>
        {
            var (action, matier, name, question, imgQuestion, rep, imgRep) = m.Value;
            Console.WriteLine(m.Value);
            question = question.Replace("\\large", "").Replace("\\(", "$").Replace("\\)", "$");
            rep = rep.Replace("\\large", "").Replace("\\(", "$").Replace("\\)", "$");
            switch (action)
            {
                case "Delete":
                    DBInteraction.DeleteCreated(question);
                    break;
                case "save":
                    DBInteraction.SaveQuizz(matier,name,question,imgQuestion,rep,imgRep);
                    break;
                case "edit":
                    _vCustomModel.EditLogic(question);
                    break;
                case "Replace":
                    _vCustomModel.ReplaceLogic(matier,name,question,rep,imgQuestion,imgRep);
                    break;
                case "Demande":
                    SendDataImport();
                    break;
            }
        });
        switch ( action)
        {
            case "Import":
                ImportLogic();
                break;
            case "Created":
                CreatedLogic();
                break;
        }
    }
    
    public void ImportLogic()
    {
        
        (_level, _course, _question, _urlQuestion, _repnse, _urlRep, _difficulty) = DBInteraction.GetAllPublic();
        // var jscode = WebviewInteraction.send_data_Card_Import(_level, _course,QuizzUtilities.Miseneformelist(_question),_urlQuestion,QuizzUtilities.Miseneformelist(_repnse),_urlRep,_difficulty);
        WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage("ClearCard()"));
    }

    public void SendDataImport()
    {
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

    public void CreatedLogic()
    {
        (_question, _repnse, _urlQuestion, _urlRep) = DBInteraction.RetrievequizzToCreated(Application.Current.Properties["nameindex"]?.ToString());
        var jscode = WebviewInteraction.send_data_Card_Created(QuizzUtilities.Miseneformelist(_question),QuizzUtilities.Miseneformelist(_repnse),_urlQuestion,_urlRep);
        Console.WriteLine(jscode);
        WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscode));
    }
    
}