using client.Common;
using client.Model;
using client.Results;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace client.Components
{
    public class PatientElementVM : INotifyPropertyChanged
    {
        private Frame _mainMenuFrame;
        private GetPatientWithAddressItemList _patientWithAddressItemList;

        private string _FIO;
        private string _address;
        private string _dateOfBirth;
        private string _gender;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GoToPatientProfileCommand { get; }

        public PatientElementVM(Frame mainMenuFrame, GetPatientWithAddressItemList patientWithAddressItemList)
        {
            _mainMenuFrame = mainMenuFrame;
            _patientWithAddressItemList = patientWithAddressItemList;

            GoToPatientProfileCommand = new RelayCommand(GoToPatientProfile);
        }

        public string FIO
        {
            get { return _FIO; }
            set
            {
                _FIO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FIO)));
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Address)));
            }
        }

        public string DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                _dateOfBirth = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateOfBirth)));
            }
        }

        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Gender)));
            }
        }

        private void GoToPatientProfile(object parameter)
        {

        }

        public void SetInfo()
        {
            FIO = _patientWithAddressItemList.AdultPatient.GetFullName;
            Address = _patientWithAddressItemList.Address.GetFullAddress;
            DateOfBirth = _patientWithAddressItemList.AdultPatient.DateOfBirth.ToString("dd.MM.yyyy");
            Gender = _patientWithAddressItemList.AdultPatient.Gender.Equals("male") ? "Мужчина" : "Женщина";
        }
    }
}
