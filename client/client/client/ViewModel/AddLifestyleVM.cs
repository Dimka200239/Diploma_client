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
    public class AddLifestyleVM : INotifyPropertyChanged
    {
        private Frame _mainMenuFrame;
        private GetPatientWithAddressItemList _patientWithAddressItemList;

        private readonly string _baseServerAdress;
        private static HttpClient client;
        private IConfiguration configuration;

        private string _smoke;
        private string _drink;
        private string _sport;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddCommand { get; }
        public ICommand GetBackCommand { get; }

        public AddLifestyleVM(Frame mainMenuFrame, GetPatientWithAddressItemList patientWithAddressItemList)
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

            Smoke = "Нет";
            Drink = "Нет";
            Sport = "Нет";
        }

        public string Smoke
        {
            get { return _smoke; }
            set
            {
                _smoke = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Smoke)));
            }
        }

        public string Drink
        {
            get { return _drink; }
            set
            {
                _drink = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Drink)));
            }
        }

        public string Sport
        {
            get { return _sport; }
            set
            {
                _sport = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sport)));
            }
        }

        private void GetBack(object parameter)
        {
            _mainMenuFrame.Content = new AdultPatientProfileView(_mainMenuFrame, _patientWithAddressItemList);
        }

        private async void Add(object parameter)
        {
            var newLifestyle = new Lifestyle()
            {
                PatientId = _patientWithAddressItemList.AdultPatient.Id,
                Role = _patientWithAddressItemList.AdultPatient.Role,
                SmokeCigarettes = Smoke.Equals("Нет") ? false : true,
                DrinkAlcohol = Drink.Equals("Нет") ? false : true,
                Sport = Sport.Equals("Нет") ? false : true,
                DateOfChange = DateTime.UtcNow
            };

            try
            {
                new Common.DataValidationContext().Validate(newLifestyle);

                var createLifestyleRequest = new LifestyleRequest
                {
                    PatientId = newLifestyle.PatientId,
                    Role = newLifestyle.Role,
                    SmokeCigarettes = newLifestyle.SmokeCigarettes,
                    DrinkAlcohol = newLifestyle.DrinkAlcohol,
                    Sport = newLifestyle.Sport
                };

                var createLifestyleResponse = await client.PostAsJsonAsync($"/api/lifestyle/createLifestyle", createLifestyleRequest);

                if (createLifestyleResponse.IsSuccessStatusCode)
                {
                    string createLifestyleResponseContent = await createLifestyleResponse.Content.ReadAsStringAsync();
                    var createLifestyleResult = JsonConvert.DeserializeObject<CreateLifestyleResult>(createLifestyleResponseContent);

                    if (createLifestyleResult.Success == true)
                    {
                        MessageBox.Show("Новые данные о полезных/вредных привычках успешно добавлены", "Успешное добавление", MessageBoxButton.OK, MessageBoxImage.Information);

                        _mainMenuFrame.Content = new AdultPatientProfileView(_mainMenuFrame, _patientWithAddressItemList);
                    }
                    else
                    {
                        MessageBox.Show(createLifestyleResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
