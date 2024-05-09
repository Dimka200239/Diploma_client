using client.Model;
using client.ViewModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace client.View
{
    /// <summary>
    /// Логика взаимодействия для AddNewAddressView.xaml
    /// </summary>
    public partial class AddNewAddressView : Page
    {
        public AddNewAddressView(AdultPatient adultPatient, Passport passport, Frame mainMenuFrame)
        {
            InitializeComponent();
            DataContext = new AddNewAddressVM(adultPatient, passport, mainMenuFrame);
        }

        private void HouseAndApartmentTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Проверяем, чтобы вводились только цифры
            e.Handled = !Regex.IsMatch(e.Text, "[0-9]");
        }

        private void RestrictedTextBox_Pasting(object sender, System.Windows.DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
    }
}
