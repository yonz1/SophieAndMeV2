
using System.Windows.Controls;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour AllQuizzSelect.xaml
    /// </summary>
    public partial class AllQuizzSelect : UserControl
    {
        public AllQuizzSelect(MainViewModel mainVm)
        {
            InitializeComponent();
            this.DataContext = new AllQuizzSelecteModel(mainVm);
        }
    }
}
