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
    /// Логика взаимодействия для FindPatientByPassportView.xaml
    /// </summary>
    public partial class FindPatientByPassportView : Page
    {
        public FindPatientByPassportView(Frame mainMenuFrame)
        {
            InitializeComponent();
            DataContext = new FindPatientByPassportVM(mainMenuFrame);

            this.MouseDown += MainWindow_MouseDown;
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Если Popup открыт и клик произошел вне Popup, то закрыть его
            if (PassDatePopupCalendar.IsOpen && !PassDatePopupCalendar.IsMouseOver)
            {
                PassDatePopupCalendar.IsOpen = false;
            }
        }

        private void Passport_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, чтобы вводились только цифры
            e.Handled = !Regex.IsMatch(e.Text, "[0-9]");
        }

        private void MyTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            PassDatePopupCalendar.IsOpen = false;
        }

        private void RestrictedTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }

        private void PassDateTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PassDatePopupCalendar.IsOpen = true;
        }

        private void RestrictedTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
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
