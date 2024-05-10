using client.Common;
using client.Components;
using client.Requests;
using client.Results;
using client.View;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace client.ViewModel
{
    public class FindPatientVM : INotifyPropertyChanged
    {
        private Frame _mainMenuFrame;
        public ObservableCollection<UserControl> ItemsList { get; private set; }

        private string _searchPatientText;

        private readonly string _baseServerAdress;
        private static HttpClient client;
        private IConfiguration configuration;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SearchPatientCommand { get; }
        public ICommand SearchPatientByPassportCommand { get; }
        public ICommand AddNewPatientCommand { get; }

        public FindPatientVM(Frame mainMenuFrame)
        {
            client = new HttpClient();

            _mainMenuFrame = mainMenuFrame;
            ItemsList = new ObservableCollection<UserControl>();

            SearchPatientCommand = new RelayCommand(SearchPatient);
            SearchPatientByPassportCommand = new RelayCommand(SearchPatientByPassport);
            AddNewPatientCommand = new RelayCommand(AddNewPatient);

            configuration = App.Instance.Configuration;

            _baseServerAdress = configuration.GetValue<string>("HttpsBaseServerAdress");
            client.BaseAddress = new Uri(_baseServerAdress);

            var employeeToken = configuration.GetValue<string>("EmployeeToken");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", employeeToken);
        }

        public string SearchPatientText
        {
            get { return _searchPatientText; }
            set
            {
                _searchPatientText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchPatientText)));
            }
        }

        private async void SearchPatient(object parameter)
        {
            if (SearchPatientText is null || SearchPatientText.Equals(""))
            {
                MessageBox.Show("Нужно ввести хотя бы 1 символ для корректного поиска", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var getAdultPatientByNameRequest = new GetAdultPatientByNameRequest()
                {
                    Name = SearchPatientText
                };

                var getAdultPatientByNameResponse = await client.PostAsJsonAsync($"/api/adultPatient/getAdultPatientByName", getAdultPatientByNameRequest);

                if (getAdultPatientByNameResponse.IsSuccessStatusCode)
                {
                    string getAdultPatientByNameResponseContent = await getAdultPatientByNameResponse.Content.ReadAsStringAsync();
                    var getAdultPatientByNameResult = JsonConvert.DeserializeObject<GetAdultPatientByNameResult>(getAdultPatientByNameResponseContent);

                    if (getAdultPatientByNameResult.Success == true)
                    {
                        ItemsList.Clear();

                        foreach (var item in getAdultPatientByNameResult.AdultPatients)
                        {
                            var newPatientElement = new PatientElement(_mainMenuFrame, item);
                            newPatientElement.MyContext.SetInfo();

                            ItemsList.Add(newPatientElement);
                        }
                    }
                    else
                    {
                        MessageBox.Show(getAdultPatientByNameResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void SearchPatientByPassport(object parameter)
        {
            _mainMenuFrame.Content = new FindPatientByPassportView(_mainMenuFrame);
        }

        private void AddNewPatient(object parameter)
        {
            _mainMenuFrame.Content = new AddNewPatientView(_mainMenuFrame);
        }
    }
}
