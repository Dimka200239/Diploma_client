using client.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace client.View
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginView : Page
    {
        public LoginView(Frame MainFrame, Window mainWindow)
        {
            InitializeComponent();
            DataContext = new UserLoginAndRegistrationPageVM(MainFrame, mainWindow);
        }
    }
}
