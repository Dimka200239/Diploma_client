using client.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace client.View
{
    /// <summary>
    /// Логика взаимодействия для AddNewPatientView.xaml
    /// </summary>
    public partial class AddNewPatientView : Page
    {
        public AddNewPatientView(Frame mainMenuFrame)
        {
            InitializeComponent();
            DataContext = new AddNewPatientVM(mainMenuFrame);

            this.MouseDown += MainWindow_MouseDown;
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

        private void BirthDateTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PopupCalendar.IsOpen = true;
        }

        private void BirthDatePicker_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar calendar = sender as Calendar;
            if (calendar.SelectedDate.HasValue)
            {
                BirthDateTextBox.Text = calendar.SelectedDate.Value.ToString("dd.MM.yyyy");
                PopupCalendar.IsOpen = false;
            }
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Если Popup открыт и клик произошел вне Popup, то закрыть его
            if (PopupCalendar.IsOpen && !PopupCalendar.IsMouseOver)
            {
                PopupCalendar.IsOpen = false;
            }

            if (PassDatePopupCalendar.IsOpen && !PassDatePopupCalendar.IsMouseOver)
            {
                PassDatePopupCalendar.IsOpen = false;
            }
        }

        private void MyTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            PopupCalendar.IsOpen = false;
            PassDatePopupCalendar.IsOpen = false;
        }

        private void RestrictedTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void RestrictedTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }

        private void Passport_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, чтобы вводились только цифры
            e.Handled = !Regex.IsMatch(e.Text, "[0-9]");
        }

        private void PassDateTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PassDatePopupCalendar.IsOpen = true;
        }

        private void PassDatePicker_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar calendar = sender as Calendar;
            if (calendar.SelectedDate.HasValue)
            {
                PassDateTextBox.Text = calendar.SelectedDate.Value.ToString("dd.MM.yyyy");
                PassDatePopupCalendar.IsOpen = false;
            }
        }
    }
}
