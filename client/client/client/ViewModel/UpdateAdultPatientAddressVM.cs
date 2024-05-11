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
    public class UpdateAdultPatientAddressVM : INotifyPropertyChanged
    {
        private Frame _mainMenuFrame;
        private GetPatientWithAddressItemList _patientWithAddressItemList;

        private readonly string _baseServerAdress;
        private static HttpClient client;
        private IConfiguration configuration;

        private string _city;
        private string _street;
        private string _house;
        private string _apartment;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddCommand { get; }
        public ICommand GetBackCommand { get; }

        public UpdateAdultPatientAddressVM(Frame mainMenuFrame, GetPatientWithAddressItemList patientWithAddressItemList)
        {
            client = new HttpClient();

            configuration = App.Instance.Configuration;

            _baseServerAdress = configuration.GetValue<string>("HttpsBaseServerAdress");
            client.BaseAddress = new Uri(_baseServerAdress);

            var employeeToken = configuration.GetValue<string>("EmployeeToken");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", employeeToken);

            _mainMenuFrame = mainMenuFrame;
            _patientWithAddressItemList = patientWithAddressItemList;

            AddCommand = new RelayCommand(Add);
            GetBackCommand = new RelayCommand(GetBack);
        }

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(City)));
            }
        }

        public string Street
        {
            get { return _street; }
            set
            {
                _street = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Street)));
            }
        }

        public string House
        {
            get { return _house; }
            set
            {
                _house = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(House)));
            }
        }

        public string Apartment
        {
            get { return _apartment; }
            set
            {
                _apartment = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Apartment)));
            }
        }

        private void GetBack(object parameter)
        {
            _mainMenuFrame.Content = new AdultPatientProfileView(_mainMenuFrame, _patientWithAddressItemList);
        }

        private async void Add(object parameter)
        {
            if (House == null || House.Equals(""))
            {
                MessageBox.Show("Вы не ввели номер дома", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newAddress = new Address()
            {
                PatientId = _patientWithAddressItemList.AdultPatient.Id,
                Role = _patientWithAddressItemList.AdultPatient.Role,
                City = City,
                Street = Street,
                House = int.Parse(House),
                Apartment = (Apartment is null || Apartment.Equals("")) ? default(int?) : int.Parse(Apartment),
                DateOfChange = DateTime.UtcNow
            };

            try
            {
                new Common.DataValidationContext().Validate(newAddress);

                var addressRequest = new CreateAddressRequest()
                {

                    PatientId = newAddress.PatientId,
                    Role = newAddress.Role,
                    City = newAddress.City,
                    Street = newAddress.Street,
                    House = newAddress.House,
                    Apartment = newAddress.Apartment
                };

                var createAddressResponse = await client.PostAsJsonAsync($"/api/address/createAddress", addressRequest);

                if (createAddressResponse.IsSuccessStatusCode)
                {
                    string createAddressResponseContent = await createAddressResponse.Content.ReadAsStringAsync();
                    var createAddressResult = JsonConvert.DeserializeObject<CreateAddressResult>(createAddressResponseContent);

                    if (createAddressResult.Success == true)
                    {
                        _patientWithAddressItemList.Address = createAddressResult.Address;

                        MessageBox.Show("Новый адрес успешно добавлен", "Успешное добавление", MessageBoxButton.OK, MessageBoxImage.Information);

                        _mainMenuFrame.Content = new AdultPatientProfileView(_mainMenuFrame, _patientWithAddressItemList);
                    }
                    else
                    {
                        MessageBox.Show(createAddressResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
