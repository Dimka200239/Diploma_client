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
using System.Xml.Linq;

namespace client.ViewModel
{
    public class AddNewAddressVM : INotifyPropertyChanged
    {
        private AdultPatient _adultPatient;
        private Passport _passport;
        private Frame _mainMenuFrame;

        private string _city;
        private string _street;
        private string _house;
        private string _apartment;

        private readonly string _baseServerAdress;
        private static HttpClient client;
        private IConfiguration configuration;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddCommand { get; }

        public AddNewAddressVM(AdultPatient adultPatient, Passport passport, Frame mainMenuFrame)
        {
            client = new HttpClient();

            configuration = App.Instance.Configuration;

            _adultPatient = adultPatient;
            _passport = passport;
            _mainMenuFrame = mainMenuFrame;

            _baseServerAdress = configuration.GetValue<string>("HttpsBaseServerAdress");
            client.BaseAddress = new Uri(_baseServerAdress);

            AddCommand = new RelayCommand(Add);
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

        private async void Add(object parameter)
        {
            if (House != null && House != "")
            {
                var newAddress = new Address();
                newAddress.Role = _adultPatient.Role;
                newAddress.City = City;
                newAddress.Street = Street;
                newAddress.House = int.Parse(House);
                newAddress.Apartment = (Apartment is null || Apartment.Equals("")) ? default(int?) : int.Parse(Apartment);
                newAddress.DateOfChange = DateTime.UtcNow;

                try
                {
                    new Common.DataValidationContext().Validate(newAddress);

                    var adultPatientRequest = new CreateAdultPatientRequest()
                    {
                        Name = _adultPatient.Name,
                        MiddleName = _adultPatient.MiddleName,
                        LastName = _adultPatient.LastName,
                        DateOfBirth = _adultPatient.DateOfBirth,
                        PhoneNumber = _adultPatient.PhoneNumber,
                        Gender = _adultPatient.Gender
                    };

                    try
                    {
                        var employeeToken = configuration.GetValue<string>("EmployeeToken");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", employeeToken);

                        var createAdultPatientResponse = await client.PostAsJsonAsync($"/api/adultPatient/createAdultPatient", adultPatientRequest);

                        if (createAdultPatientResponse.IsSuccessStatusCode)
                        {
                            string createAdultPatientResponseContent = await createAdultPatientResponse.Content.ReadAsStringAsync();
                            var createAdultPatientResult = JsonConvert.DeserializeObject<CreateAdultPatientResult>(createAdultPatientResponseContent);

                            if (createAdultPatientResult.Success == true)
                            {
                                var passportRequest = new CreatePassportRequest()
                                {
                                    AdultPatientId = createAdultPatientResult.AdultPatientId,
                                    Series = _passport.Series,
                                    Number = _passport.Number,
                                    Code = _passport.Code,
                                    DateOfIssue = _passport.DateOfIssue
                                };

                                var createPassportResponse = await client.PostAsJsonAsync($"/api/passport/createPassport", passportRequest);

                                if (createPassportResponse.IsSuccessStatusCode)
                                {
                                    string createPassportResponseContent = await createPassportResponse.Content.ReadAsStringAsync();
                                    var createPassportResult = JsonConvert.DeserializeObject<CreatePassportResult>(createPassportResponseContent);

                                    if (createPassportResult.Success == true)
                                    {
                                        var addressRequest = new CreateAddressRequest()
                                        {
                                            PatientId = createAdultPatientResult.AdultPatientId,
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
                                                MessageBox.Show("Пациент, его паспорт и адрес успешно добавлены!", "Данные успешно добавлены в базу данных", MessageBoxButton.OK, MessageBoxImage.Information);
                                                _mainMenuFrame.Content = new FindPatientView(_mainMenuFrame);
                                            }
                                            else
                                            {
                                                MessageBox.Show(createAddressResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(createPassportResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show(createAdultPatientResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Вы не ввели номер дома", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
