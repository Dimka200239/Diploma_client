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
    /// Логика взаимодействия для UpdateInfoAboutAdultPatientView.xaml
    /// </summary>
    public partial class UpdateInfoAboutAdultPatientView : Page
    {
        public UpdateInfoAboutAdultPatientView(Frame mainMenuFrame, GetPatientWithAddressItemList patientWithAddressItemList)
        {
            InitializeComponent();
            DataContext = new UpdateInfoAboutAdultPatientVM(mainMenuFrame, patientWithAddressItemList);
        }

        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, чтобы вводились только цифры
            e.Handled = !Regex.IsMatch(e.Text, "[0-9]");

            // Дополнительно проверяем, чтобы первый символ был 8
            var textBox = sender as TextBox;
            if (textBox.Text.Length == 0 && e.Text != "8")
            {
                e.Handled = true;
            }
        }

        private void RestrictedTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
    }
}
