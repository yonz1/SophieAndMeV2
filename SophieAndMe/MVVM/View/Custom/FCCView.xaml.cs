using System.Windows.Controls;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View.Custom;

public partial class FCCView : UserControl
{
    public FCCView(MainViewModel mainVm)
    {
        InitializeComponent();
        this.DataContext = new FFCViewModel();
    }
}