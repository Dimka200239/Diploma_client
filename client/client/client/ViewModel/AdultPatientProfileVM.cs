using client.Common;
using client.Results;
using client.View;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace client.ViewModel
{
    public class AdultPatientProfileVM : INotifyPropertyChanged
    {
        private Frame _mainMenuFrame;
        private GetPatientWithAddressItemList _patientWithAddressItemList;

        private readonly string _baseServerAdress;
        private static HttpClient client;
        private IConfiguration configuration;

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
            client = new HttpClient();

            configuration = App.Instance.Configuration;

            _baseServerAdress = configuration.GetValue<string>("HttpsBaseServerAdress");
            client.BaseAddress = new Uri(_baseServerAdress);

            var employeeToken = configuration.GetValue<string>("EmployeeToken");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", employeeToken);

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

        private async void GetAnalisys(object parameter)
        {
            try
            {
                var getInfoAboutAdultPatientResponse = await client.GetAsync($"/api/adultPatient/getAdultPatientByIdWithAnthropometryAndLifestyle/{_patientWithAddressItemList.AdultPatient.Id}");

                if (getInfoAboutAdultPatientResponse.IsSuccessStatusCode)
                {
                    string getInfoAboutAdultPatientResponseContent = await getInfoAboutAdultPatientResponse.Content.ReadAsStringAsync();
                    var getInfoAboutAdultPatientResult = JsonConvert.DeserializeObject<GetAdultPatientByIdWithAnthropometryAndLifestyleResult>(getInfoAboutAdultPatientResponseContent);

                    if (getInfoAboutAdultPatientResult.Success == true)
                    {
                        if (getInfoAboutAdultPatientResult.AnthropometryOfPatient is null)
                        {
                            MessageBox.Show("Добавьте актуальные антропометрические данные!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        if (getInfoAboutAdultPatientResult.AnthropometryOfPatient.DateOfChange.Date != DateTime.Today.Date)
                        {
                            MessageBox.Show("Добавьте актуальные антропометрические данные!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        if (getInfoAboutAdultPatientResult.Lifestyle is null)
                        {
                            MessageBox.Show("Добавьте актуальные данные о вредных/полезных привычках!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        if (getInfoAboutAdultPatientResult.Lifestyle.DateOfChange.Date != DateTime.Today.Date)
                        {
                            MessageBox.Show("Добавьте актуальные данные о вредных/полезных привычках!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        _mainMenuFrame.Content = new CountSSZView(_mainMenuFrame, _patientWithAddressItemList, getInfoAboutAdultPatientResult.AnthropometryOfPatient, getInfoAboutAdultPatientResult.Lifestyle);
                    }
                    else
                    {
                        MessageBox.Show(getInfoAboutAdultPatientResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
