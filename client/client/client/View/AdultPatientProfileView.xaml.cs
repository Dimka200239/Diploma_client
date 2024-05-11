using client.Results;
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
    /// Логика взаимодействия для AdultPatientProfileView.xaml
    /// </summary>
    public partial class AdultPatientProfileView : Page
    {
        public AdultPatientProfileView(Frame mainMenuFrame, GetPatientWithAddressItemList patientWithAddressItemList)
        {
            InitializeComponent();
            DataContext = new AdultPatientProfileVM(mainMenuFrame, patientWithAddressItemList);
        }
    }
}
