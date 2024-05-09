using client.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace client.View
{
    /// <summary>
    /// Логика взаимодействия для FindPatientView.xaml
    /// </summary>
    public partial class FindPatientView : Page
    {
        public FindPatientView(Frame MainMenuFrame)
        {
            InitializeComponent();
            DataContext = new FindPatientVM(MainMenuFrame);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
