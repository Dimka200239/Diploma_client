using client.ViewModel;
using System.Windows.Controls;

namespace client.View
{
    /// <summary>
    /// Логика взаимодействия для AddNewDataLearningView.xaml
    /// </summary>
    public partial class AddNewDataLearningView : Page
    {
        public AddNewDataLearningView()
        {
            InitializeComponent();
            DataContext = new AddNewDataLearningVM();
        }
    }
}
