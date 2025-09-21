using System.ComponentModel;
using System.Runtime.CompilerServices;
using SophieAndMe.MVVM.Model;

namespace SophieAndMe.MVVM.ViewModel;

public class FFCViewModel : INotifyPropertyChanged
{
    
    private string _selectedmat;

    public string SelectedMat
    {
        get => _selectedmat;
        set
        {
            if (_selectedmat != value)
            {
                _selectedmat = value;
            }
        }
    }
    
    private string _selectedchap;

    public string SelectedChap
    {
        get => _selectedchap;
        set
        {
            if (_selectedchap != value)
            {
                _selectedchap = value;
            }
        }
    }
    public List<string> Mat { get; } = new();
    public List<string> Chapitre { get; } = new();

    public FFCViewModel()
    {
        Mat = ["Physique", "Mathématiques", "Français", "Anglais", "Erreurs", "SI"];
        Chapitre = DbInteraction.GetAllName();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}