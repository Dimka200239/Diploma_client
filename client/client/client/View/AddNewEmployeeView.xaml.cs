using client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace client.View
{
    /// <summary>
    /// Логика взаимодействия для AddNewEmployeeView.xaml
    /// </summary>
    public partial class AddNewEmployeeView : Page
    {
        public AddNewEmployeeView()
        {
            InitializeComponent();
            DataContext = new AddNewEmployeeVM();
        }
    }
}
