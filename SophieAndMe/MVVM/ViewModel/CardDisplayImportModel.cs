using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using SophieAndMe.Core;
using SophieAndMe.MVVM.Model;
using SophieAndMe.Windows;

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
    private IDataService _dataService;
    Dictionary<string, string> dico = new Dictionary<string, string>();

    public class CardMessage
    {
        public string Action { get; set; }
        public string Question { get; set; }
        public string Matiere { get; set; }
        public string Name { get; set; }
        public string ImgQuestion { get; set; }
        public string Rep { get; set; }
        public string ImgRep { get; set; }
    }


    public CardDisplayImportModel(VCustomModel vm,string action)
    {
        _vCustomModel = vm;
        _dataService =  App.DataService;
        WeakReferenceMessenger.Default.Register<MediatorCustom.JstoAppMessage>(this, (r, m) =>
        {
            Console.WriteLine("Debut " +  _dataService.WinBin.ToString());
            var (action, matier, name, question, imgQuestion, rep, imgRep) = m.Value;
            question = question.Replace("\\large", "").Replace("\\(", "$").Replace("\\)", "$");
            rep = rep.Replace("\\large", "").Replace("\\(", "$").Replace("\\)", "$");
            switch (action)
            {
                case "Delete":
                    DbInteraction.DeleteCreated(question);
                    break;
                case "save":
                    DbInteraction.SaveQuizz(matier,name,question,imgQuestion,rep,imgRep);
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
                case "Add":
                    Console.WriteLine("Avant " + _dataService.WinBin.ToString());
                    if (!_dataService.WinBin)
                    {
                        Console.WriteLine("Windows charger");
                        _dataService.WinBin = true;
                        ImportAdd win = new ImportAdd(vm,question,imgQuestion,rep,imgRep);
                        win.ShowDialog();
                    }
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
        (_level, _course, _question, _urlQuestion, _repnse, _urlRep, _difficulty) = DbInteraction.GetAllPublic();
        _dataService.IdCard.Number += 1;
        dico["level"] = _level[0];
        dico["Action"] = "Test";
        dico["Id"] = _dataService.IdCard.Number.ToString();
        string jscode = JsonSerializer.Serialize(dico);
        WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscode));
    }
    

    public void SendDataImport()
    {
        Console.WriteLine(_level.Count);
        for (int i = 0; i < _level.Count; i++)
        {
            dico["level"] = _level[i];
            dico["course"] = _course[i];
            dico["question"] = _question[i];
            dico["repnse"] = _repnse[i];
            dico["urlQuestion"] = _urlQuestion[i];
            dico["urlRep"] = _urlRep[i];
            dico["difficulty"] = _difficulty[i];
            dico["len"] = _question.Count.ToString();
            dico["Action"] = "Import";
            dico["Id"] = _dataService.IdCard.Number.ToString();
            string jscode = JsonSerializer.Serialize(dico); 
            WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscode));
        }
    }

    public void CreatedLogic()
    {
        (_question, _repnse, _urlQuestion, _urlRep) = DbInteraction.RetrievequizzToCreated(Application.Current.Properties["nameindex"]?.ToString());
        var q = QuizzUtilities.Miseneformelist(_question) ?? new List<string>();
        var r = QuizzUtilities.Miseneformelist(_repnse) ?? new List<string>();
        var uq = QuizzUtilities.Miseneformelist(_urlQuestion) ?? new List<string>();
        var ur = QuizzUtilities.Miseneformelist(_urlRep) ?? new List<string>();
        ShowCard(q, r, uq, ur);
    }
    private async void ShowCard(List<string> question,List<string> reponse,List<string> urlQuestion,List<string> urlReponse)
    {
        _dataService.IdCard.Number += 1;
        Dictionary<string, string> dico = new Dictionary<string, string>();
        for (int i = 0; i < question.Count; i++)
        {
            dico["level"] = "";
            dico["course"] = "";
            dico["question"] = question[i];
            dico["repnse"] = reponse[i];
            dico["urlQuestion"] = urlQuestion[i];
            dico["urlRep"] = urlReponse[i];
            dico["difficulty"] = "";
            dico["len"] = question.Count.ToString();
            dico["Action"] = "Created";
            dico["Id"] = _dataService.IdCard.Number.ToString();
            string jscode = JsonSerializer.Serialize(dico);
            WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscode));
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    
}