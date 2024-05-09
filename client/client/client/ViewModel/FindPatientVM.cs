using client.Common;
using client.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace client.ViewModel
{
    public class FindPatientVM : INotifyPropertyChanged
    {
        private Frame _mainMenuFrame;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SearchPatientCommand { get; }
        public ICommand SearchPatientByPassportCommand { get; }
        public ICommand AddNewPatientCommand { get; }

        public FindPatientVM(Frame mainMenuFrame)
        {
            _mainMenuFrame = mainMenuFrame;

            SearchPatientCommand = new RelayCommand(SearchPatient);
            SearchPatientByPassportCommand = new RelayCommand(SearchPatientByPassport);
            AddNewPatientCommand = new RelayCommand(AddNewPatient);
        }

        private void SearchPatient(object parameter)
        {

        }

        private void SearchPatientByPassport(object parameter)
        {

        }

        private void AddNewPatient(object parameter)
        {
            _mainMenuFrame.Content = new AddNewPatientView(_mainMenuFrame);
        }
    }
}
