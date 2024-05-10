using client.Model;
using client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace client.View
{
    /// <summary>
    /// Логика взаимодействия для MainEmployeeMenu.xaml
    /// </summary>
    public partial class MainEmployeeMenu : Page
    {
        private MainEmployeeMenuVM _myContex;

        public MainEmployeeMenu(Employee employee, Frame MainFrame)
        {
            InitializeComponent();
            var nav = NavigationService.GetNavigationService(MainMenuFrame);
            _myContex = new MainEmployeeMenuVM(employee, MainFrame, MainMenuFrame);
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
