using System.Windows.Controls;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour EndQuizz.xaml
    /// </summary>
    public partial class EndQuizz : UserControl
    {
        public EndQuizz(EndQuizzModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
