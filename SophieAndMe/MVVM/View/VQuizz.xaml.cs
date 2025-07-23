using SophieAndMe.MVVM.ViewModel;
using System.Windows.Controls;


namespace SophieAndMe.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour VQuizz.xaml
    /// </summary>
    public partial class VQuizz : UserControl
    {
        public VQuizz()
        {
            InitializeComponent();
            this.DataContext = new VQuizzModel();
        }
    }
}
