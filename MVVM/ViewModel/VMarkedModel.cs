using System.Windows;
using System.Windows.Input;
using SophieAndMe.MVVM.Model;

namespace SophieAndMe.MVVM.ViewModel;

public class VMarkedModel
{
    
    List<string>? _question; 
    List<string>? _repnse; 
    List<string>? _urlQuestion; 
    private List<string>? _urlRep;
    private readonly Func<string, Task> _invokejs ;
    private string _jscall;
    public ICommand ShowMaths { get; }
    public ICommand ShowPhysique { get; }
    public ICommand ShowSI {  get; }
    public ICommand ShowFrançais {  get; }
    public ICommand ShowAnglais { get; }
    public ICommand ShowAll {  get; }
    public ICommand ShowErreurs { get; }
    
    public VMarkedModel(Func<string, Task> invokeJs)
    {
        _invokejs = invokeJs;
        ShowMaths = new RelayCommand(o =>
        {
            LoadMark("Mathématiques");
        });
        
        ShowPhysique = new RelayCommand(o =>
        {
            LoadMark("Physique");
        });
        
        ShowSI= new RelayCommand(o =>
        {
            LoadMark("SI");
        });
        
        ShowFrançais = new RelayCommand(o =>
        {
            LoadMark("Français");
        });
        
        ShowAnglais = new RelayCommand(o =>
        {
            LoadMark("Anglais");
        });
        
        ShowErreurs = new RelayCommand(o =>
        {
            LoadMark("Erreurs");
        });
        
        ShowAll = new RelayCommand(o =>
        {
            LoadMark("all");
        });
    }

    public async void LoadMark(string mat)
    {
        (_question, _repnse, _urlQuestion, _urlRep) = DBInteraction.GetMarked(mat);
        var q = QuizzUtilities.Miseneformelist(_question) ?? new List<string>();
        var r = QuizzUtilities.Miseneformelist(_repnse) ?? new List<string>();
        var uq = QuizzUtilities.Miseneformelist(_urlQuestion) ?? new List<string>();
        var ur = QuizzUtilities.Miseneformelist(_urlRep) ?? new List<string>();

        try
        {
            _jscall = WebviewInteraction.send_data_Card_Marked(q, r, uq, ur);
            await _invokejs(_jscall);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            MessageBox.Show("An error occured while loading your Data, please retry later");
        }
    }
}