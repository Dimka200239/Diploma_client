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
    public class AddAnthropometryOfPatientsVM : INotifyPropertyChanged
    {
        private Frame _mainMenuFrame;
        private GetPatientWithAddressItemList _patientWithAddressItemList;

        private readonly string _baseServerAdress;
        private static HttpClient client;
        private IConfiguration configuration;

        private string _height;
        private string _weight;
        private string _age;
        private string _waist;
        private string _hip;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddCommand { get; }
        public ICommand GetBackCommand { get; }

        public AddAnthropometryOfPatientsVM(Frame mainMenuFrame, GetPatientWithAddressItemList patientWithAddressItemList)
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

            DateTime today = DateTime.Today;
            int realAge = today.Year - _patientWithAddressItemList.AdultPatient.DateOfBirth.Year;
            if (_patientWithAddressItemList.AdultPatient.DateOfBirth > today.AddYears(-realAge))
                realAge--;

            Age = realAge.ToString();
        }

        public string Height
        {
            get { return _height; }
            set
            {
                _height = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height)));
            }
        }

        public string Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weight)));
            }
        }

        public string Age
        {
            get { return _age; }
            set
            {
                _age = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Age)));
            }
        }

        public string Waist
        {
            get { return _waist; }
            set
            {
                _waist = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Waist)));
            }
        }

        public string Hip
        {
            get { return _hip; }
            set
            {
                _hip = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Hip)));
            }
        }

        private void GetBack(object parameter)
        {
            _mainMenuFrame.Content = new AdultPatientProfileView(_mainMenuFrame, _patientWithAddressItemList);
        }

        private async void Add(object parameter)
        {
            if (Height == null || Height.Equals(""))
            {
                MessageBox.Show("Вы не ввели рост", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Weight == null || Weight.Equals(""))
            {
                MessageBox.Show("Вы не ввели вес", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Waist == null || Waist.Equals(""))
            {
                MessageBox.Show("Вы не ввели объем талии", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Hip == null || Hip.Equals(""))
            {
                MessageBox.Show("Вы не ввели объем бедра", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newAnthropometryOfPatient = new AnthropometryOfPatient()
            {
                PatientId = _patientWithAddressItemList.AdultPatient.Id,
                Role = _patientWithAddressItemList.AdultPatient.Role,
                Height = double.Parse(Height),
                Weight = double.Parse(Weight),
                Age = int.Parse(Age),
                Waist = double.Parse(Waist),
                Hip = double.Parse(Hip),
                DateOfChange = DateTime.UtcNow
            };

            try
            {
                new Common.DataValidationContext().Validate(newAnthropometryOfPatient);

                var createAnthropometryOfPatientRequest = new CreateAnthropometryOfPatientRequest
                {
                    PatientId = newAnthropometryOfPatient.PatientId,
                    Role = newAnthropometryOfPatient.Role,
                    Height = newAnthropometryOfPatient.Height,
                    Weight = newAnthropometryOfPatient.Weight,
                    Age = newAnthropometryOfPatient.Age,
                    Waist = newAnthropometryOfPatient.Waist,
                    Hip = newAnthropometryOfPatient.Hip
                };

                var createAnthropometryOfPatientResponse = await client.PostAsJsonAsync($"/api/anthropometryOfPatient/createAnthropometryOfPatient", createAnthropometryOfPatientRequest);

                if (createAnthropometryOfPatientResponse.IsSuccessStatusCode)
                {
                    string createAnthropometryOfPatientResponseContent = await createAnthropometryOfPatientResponse.Content.ReadAsStringAsync();
                    var createAnthropometryOfPatientResult = JsonConvert.DeserializeObject<CreateAnthropometryOfPatientResult>(createAnthropometryOfPatientResponseContent);

                    if (createAnthropometryOfPatientResult.Success == true)
                    {
                        MessageBox.Show("Новые антропометрические данные успешно добавлены", "Успешное добавление", MessageBoxButton.OK, MessageBoxImage.Information);

                        _mainMenuFrame.Content = new AdultPatientProfileView(_mainMenuFrame, _patientWithAddressItemList);
                    }
                    else
                    {
                        MessageBox.Show(createAnthropometryOfPatientResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
