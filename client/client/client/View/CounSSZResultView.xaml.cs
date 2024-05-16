using client.Model;
using client.ViewModel;
using System.Windows.Controls;

namespace client.View
{
    /// <summary>
    /// Логика взаимодействия для CounSSZResultView.xaml
    /// </summary>
    public partial class CounSSZResultView : Page
    {
        public CounSSZResultView(HealthPrediction healthPrediction)
        {
            InitializeComponent();
            DataContext = new CounSSZResultVM(healthPrediction);
        }
    }
}
