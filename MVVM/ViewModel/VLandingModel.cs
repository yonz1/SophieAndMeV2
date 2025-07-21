using System.ComponentModel;
using System.Runtime.CompilerServices;
using SophieAndMe.MVVM.Model;

namespace SophieAndMe.MVVM.ViewModel;

public class VLandingModel : INotifyPropertyChanged
{
    private readonly Func<string, Task> _invokejs;
    private readonly List<string> _question;
    private readonly List<string> _repnse;
    private readonly List<string> _urlQuestion;
    private readonly List<string> _urlRep;
    private string _jscode = null!;
    
    public VLandingModel(Func<string, Task> invokeJs)
    {
        _invokejs = invokeJs;
    }
    
    
    public event PropertyChangedEventHandler PropertyChanged = null!;
    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

