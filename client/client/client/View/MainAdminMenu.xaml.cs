using client.Model;
using client.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace client.View
{
    /// <summary>
    /// Логика взаимодействия для MainAdminMenu.xaml
    /// </summary>
    public partial class MainAdminMenu : Page
    {
        private MainAdminMenuVM _myContex;

        public MainAdminMenu(Employee employee, Frame MainFrame, Window mainWindow)
        {
            InitializeComponent();
            var nav = NavigationService.GetNavigationService(MainAdminMenuFrame);
            _myContex = new MainAdminMenuVM(employee, MainFrame, MainAdminMenuFrame, mainWindow);
            DataContext = _myContex;
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_myContex.IsDimmed == true)
            {
                _myContex.CloseSection();
            }
        }
    }
}
