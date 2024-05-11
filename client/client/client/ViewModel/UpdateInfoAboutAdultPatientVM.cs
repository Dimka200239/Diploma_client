using client.Common;
using client.Model;
using client.Requests;
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
    public class UpdateInfoAboutAdultPatientVM : INotifyPropertyChanged
    {
        private Frame _mainMenuFrame;
        private GetPatientWithAddressItemList _patientWithAddressItemList;

        private readonly string _baseServerAdress;
        private static HttpClient client;
        private IConfiguration configuration;

        private string _lastName;
        private string _name;
        private string _phone;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SaveCommand { get; }
        public ICommand GetBackCommand { get; }

        public UpdateInfoAboutAdultPatientVM(Frame mainMenuFrame, GetPatientWithAddressItemList patientWithAddressItemList)
        {
            client = new HttpClient();

            configuration = App.Instance.Configuration;

            _baseServerAdress = configuration.GetValue<string>("HttpsBaseServerAdress");
            client.BaseAddress = new Uri(_baseServerAdress);

            var employeeToken = configuration.GetValue<string>("EmployeeToken");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", employeeToken);

            _mainMenuFrame = mainMenuFrame;
            _patientWithAddressItemList = patientWithAddressItemList;

            SaveCommand = new RelayCommand(Save);
            GetBackCommand = new RelayCommand(GetBack);

            SetInfo();
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

        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Phone)));
            }
        }

        private async void Save(object parameter)
        {
            var updateAdultPatient = new AdultPatient()
            {
                Id = _patientWithAddressItemList.AdultPatient.Id,
                Name = Name,
                MiddleName = _patientWithAddressItemList.AdultPatient.MiddleName,
                LastName = LastName,
                DateOfBirth = _patientWithAddressItemList.AdultPatient.DateOfBirth,
                PhoneNumber = Phone,
                Gender = _patientWithAddressItemList.AdultPatient.Gender,
                Role = _patientWithAddressItemList.AdultPatient.Role
            };

            try
            {
                new Common.DataValidationContext().Validate(updateAdultPatient);

                var updateAdultPatientRequest = new UpdateAdultPatientRequest()
                {
                    AdultPatientId = updateAdultPatient.Id,
                    Name = updateAdultPatient.Name,
                    LastName = updateAdultPatient.LastName,
                    PhoneNumber = updateAdultPatient.PhoneNumber
                };

                var updateAdultPatientResponse = await client.PutAsJsonAsync($"/api/adultPatient/updateAdultPatient", updateAdultPatientRequest);

                if (updateAdultPatientResponse.IsSuccessStatusCode)
                {
                    string updateAdultPatientResponseContent = await updateAdultPatientResponse.Content.ReadAsStringAsync();
                    var updateAdultPatientResult = JsonConvert.DeserializeObject<UpdateAdultPatientResult>(updateAdultPatientResponseContent);

                    if (updateAdultPatientResult.Success == true)
                    {
                        _patientWithAddressItemList.AdultPatient = updateAdultPatientResult.AdultPatient;

                        MessageBox.Show("Информация о человеке успешно обновлена", "Успешное обновление", MessageBoxButton.OK, MessageBoxImage.Information);

                        _mainMenuFrame.Content = new AdultPatientProfileView(_mainMenuFrame, _patientWithAddressItemList);
                    }
                    else
                    {
                        MessageBox.Show(updateAdultPatientResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GetBack(object parameter)
        {
            _mainMenuFrame.Content = new AdultPatientProfileView(_mainMenuFrame, _patientWithAddressItemList);
        }

        public void SetInfo()
        {
            LastName = _patientWithAddressItemList.AdultPatient.LastName;
            Name = _patientWithAddressItemList.AdultPatient.Name;
            Phone = _patientWithAddressItemList.AdultPatient.PhoneNumber;
        }
    }
}
