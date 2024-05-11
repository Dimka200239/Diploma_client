using client.Results;
using client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для UpdateAdultPatientAddress.xaml
    /// </summary>
    public partial class UpdateAdultPatientAddressView : Page
    {
        public UpdateAdultPatientAddressView(Frame mainMenuFrame, GetPatientWithAddressItemList patientWithAddressItemList)
        {
            InitializeComponent();
            DataContext = new UpdateAdultPatientAddressVM(mainMenuFrame, patientWithAddressItemList);
        }

        private void HouseAndApartmentTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, чтобы вводились только цифры
            e.Handled = !Regex.IsMatch(e.Text, "[0-9]");
        }

        private void RestrictedTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
    }
}
