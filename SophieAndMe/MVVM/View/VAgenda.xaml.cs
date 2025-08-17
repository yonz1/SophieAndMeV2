using System.Windows.Controls;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View;

public partial class VAgenda : UserControl
{
    public VAgenda(MainViewModel mainVm )
    {
        
        InitializeComponent();
        this.DataContext = new VAgendaModel();
    }
}