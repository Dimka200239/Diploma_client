using client.Common;
using client.Results;
using client.View;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace client.ViewModel
{
    public class AdultPatientProfileVM : INotifyPropertyChanged
    {
        private Frame _mainMenuFrame;
        private GetPatientWithAddressItemList _patientWithAddressItemList;

        private string _FIO;
        private string _dateOfBirth;
        private string _address;
        private string _phone;
        private string _gender;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddNewAddressCommand { get; }
        public ICommand UpdatePatientInfoCommand { get; }
        public ICommand AddNewAntropometryCommand { get; }
        public ICommand UpdateLifestyleCommand { get; }
        public ICommand GetAnalisysCommand { get; }

        public AdultPatientProfileVM(Frame mainMenuFrame, GetPatientWithAddressItemList patientWithAddressItemList)
        {
            _mainMenuFrame = mainMenuFrame;
            _patientWithAddressItemList = patientWithAddressItemList;

            AddNewAddressCommand = new RelayCommand(AddNewAddress);
            UpdatePatientInfoCommand = new RelayCommand(UpdatePatientInfo);
            AddNewAntropometryCommand = new RelayCommand(AddNewAntropometry);
            UpdateLifestyleCommand = new RelayCommand(UpdateLifestyle);
            GetAnalisysCommand = new RelayCommand(GetAnalisys);

            SetInfo();
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

        public string DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                _dateOfBirth = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateOfBirth)));
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

        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Phone)));
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

        private void AddNewAddress(object parameter)
        {
            _mainMenuFrame.Content = new UpdateAdultPatientAddressView(_mainMenuFrame, _patientWithAddressItemList);
        }

        private void UpdatePatientInfo(object parameter)
        {
            _mainMenuFrame.Content = new UpdateInfoAboutAdultPatientView(_mainMenuFrame, _patientWithAddressItemList);
        }

        private void AddNewAntropometry(object parameter)
        {
            _mainMenuFrame.Content = new AddAnthropometryOfPatientsView(_mainMenuFrame, _patientWithAddressItemList);
        }

        private void UpdateLifestyle(object parameter)
        {
            _mainMenuFrame.Content = new AddLifestyleView(_mainMenuFrame, _patientWithAddressItemList);
        }

        private void GetAnalisys(object parameter)
        {

        }

        public void SetInfo()
        {
            FIO = _patientWithAddressItemList.AdultPatient.GetFullName;
            Address = _patientWithAddressItemList.Address.GetFullAddress;
            DateOfBirth = _patientWithAddressItemList.AdultPatient.DateOfBirth.ToString("dd.MM.yyyy");
            Phone = _patientWithAddressItemList.AdultPatient.PhoneNumber;
            Gender = _patientWithAddressItemList.AdultPatient.Gender.Equals("male") ? "Мужчина" : "Женщина";
        }
    }
}
