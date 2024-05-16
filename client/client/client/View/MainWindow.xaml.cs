using System.Windows;
using System.Windows.Navigation;

namespace client.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var nav = NavigationService.GetNavigationService(MainFrame);
            MainFrame.Content = new LoginView(MainFrame, this);
        }
    }
}
