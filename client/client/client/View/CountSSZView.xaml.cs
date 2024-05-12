using client.Model;
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
    /// Логика взаимодействия для CountSSZView.xaml
    /// </summary>
    public partial class CountSSZView : Page
    {
        private CountSSZVM _myContex;

        public CountSSZView(Frame mainMenuFrame, GetPatientWithAddressItemList patientWithAddressItemList,
            AnthropometryOfPatient anthropometryOfPatient, Lifestyle lifestyle)
        {
            InitializeComponent();
            _myContex = new CountSSZVM(mainMenuFrame, patientWithAddressItemList, anthropometryOfPatient, lifestyle);
            _myContex.SetInfo();
            DataContext = _myContex;
        }

        private void ValidateTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string proposedText = textBox.Text.Insert(textBox.CaretIndex, e.Text);

            // Обновленное регулярное выражение
            Regex regex = new Regex(@"^(0|([1-9]\d*))(\,\d*)?$");
            if (!regex.IsMatch(proposedText) || (e.Text == "," && textBox.Text.Contains(",")))
            {
                e.Handled = true;
            }
        }

        private void PreventSpaceAndIncorrectDot(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // Запрещаем пробел
            {
                e.Handled = true;
            }
            else if ((e.Key == Key.Decimal || e.Key == Key.OemPeriod) && ((TextBox)sender).Text.Contains(","))
            {
                // Запрещаем вторую точку
                e.Handled = true;
            }
        }

        private void RestrictedTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
    }
}
