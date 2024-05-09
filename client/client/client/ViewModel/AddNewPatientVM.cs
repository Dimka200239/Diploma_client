using client.Common;
using client.Model;
using client.View;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace client.ViewModel
{
    public class AddNewPatientVM : INotifyPropertyChanged
    {
        private Frame _mainMenuFrame;

        private string _lastName;
        private string _name;
        private string _middleName;
        private string _dateOfBirth;
        private string _phone;
        private string _gender;
        private string _passSeries;
        private string _passNumber;
        private string _passCode;
        private string _passDate;

        public ICommand NextStepCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AddNewPatientVM(Frame mainMenuFrame)
        {
            _mainMenuFrame = mainMenuFrame;

            Gender = "Мужчина";

            NextStepCommand = new RelayCommand(NextStep);
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName)));
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                _middleName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MiddleName)));
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

        public string PassSeries
        {
            get { return _passSeries; }
            set
            {
                _passSeries = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PassSeries)));
            }
        }

        public string PassNumber
        {
            get { return _passNumber; }
            set
            {
                _passNumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PassNumber)));
            }
        }

        public string PassCode
        {
            get { return _passCode; }
            set
            {
                _passCode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PassCode)));
            }
        }

        public string PassDate
        {
            get { return _passDate; }
            set
            {
                _passDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PassDate)));
            }
        }

        private void NextStep(object parameter)
        {
            if (DateOfBirth != null)
            {
                if (PassDate != null)
                {
                    string format = "dd.MM.yyyy";

                    var newAdultPatient = new AdultPatient();
                    newAdultPatient.Name = Name;
                    newAdultPatient.LastName = LastName;
                    newAdultPatient.MiddleName = MiddleName is null ? "" : MiddleName;
                    newAdultPatient.DateOfBirth = DateTime.ParseExact(DateOfBirth, format, System.Globalization.CultureInfo.InvariantCulture);
                    newAdultPatient.PhoneNumber = Phone;
                    newAdultPatient.Gender = Gender.Equals("Мужчина") == true ? client.Model.Gender.Male : client.Model.Gender.Female;
                    newAdultPatient.Role = Role.AdultPatient;

                    var newPassport = new Passport();
                    newPassport.Series = PassSeries;
                    newPassport.Number = PassNumber;
                    newPassport.Code = PassCode;
                    newPassport.DateOfIssue = DateTime.ParseExact(PassDate, format, System.Globalization.CultureInfo.InvariantCulture);
                    try
                    {
                        new Common.DataValidationContext().Validate(newAdultPatient);

                        new Common.DataValidationContext().Validate(newPassport);

                        _mainMenuFrame.Content = new AddNewAddressView(newAdultPatient, newPassport, _mainMenuFrame);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Вы не ввели дату выдачи паспорта", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Вы не ввели дату рождения", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
