using client.Common;
using client.Components;
using client.Model;
using client.Requests;
using client.Results;
using client.View;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace client.ViewModel
{
    public class FindPatientByPassportVM : INotifyPropertyChanged
    {
        private Frame _mainMenuFrame;
        public ObservableCollection<UserControl> ItemsList { get; private set; }

        private readonly string _baseServerAdress;
        private static HttpClient client;
        private IConfiguration configuration;

        private string _passSeries;
        private string _passNumber;
        private string _passDate;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SearchPatientCommand { get; }
        public ICommand SearchPatientByNameCommand { get; }
        public ICommand AddNewPatientCommand { get; }

        public FindPatientByPassportVM(Frame mainMenuFrame)
        {
            client = new HttpClient();

            _mainMenuFrame = mainMenuFrame;
            ItemsList = new ObservableCollection<UserControl>();

            SearchPatientCommand = new RelayCommand(SearchPatient);
            SearchPatientByNameCommand = new RelayCommand(SearchPatientByName);
            AddNewPatientCommand = new RelayCommand(AddNewPatient);

            configuration = App.Instance.Configuration;

            _baseServerAdress = configuration.GetValue<string>("HttpsBaseServerAdress");
            client.BaseAddress = new Uri(_baseServerAdress);

            var employeeToken = configuration.GetValue<string>("EmployeeToken");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", employeeToken);
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

        public string PassDate
        {
            get { return _passDate; }
            set
            {
                _passDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PassDate)));
            }
        }

        private async void SearchPatient(object parameter)
        {
            if (PassDate != null)
            {
                string format = "dd.MM.yyyy";

                var passport = new Passport();
                passport.Series = PassSeries;
                passport.Number = PassNumber;
                passport.Code = "------";
                passport.DateOfIssue = DateTime.ParseExact(PassDate, format, System.Globalization.CultureInfo.InvariantCulture);

                try
                {
                    new Common.DataValidationContext().Validate(passport);

                    var getAdultPatientByPassportRequest = new GetAdultPatientByPassportRequest()
                    {
                        Series = passport.Series,
                        Number = passport.Number,
                        DateOfIssue = passport.DateOfIssue
                    };

                    var getAdultPatientByPassportResponse = await client.PostAsJsonAsync($"/api/adultPatient/getAdultPatientByPassport", getAdultPatientByPassportRequest);

                    if (getAdultPatientByPassportResponse.IsSuccessStatusCode)
                    {
                        string getAdultPatientByPassportResponseContent = await getAdultPatientByPassportResponse.Content.ReadAsStringAsync();
                        var getAdultPatientByPassportResult = JsonConvert.DeserializeObject<GetAdultPatientByPassportResult>(getAdultPatientByPassportResponseContent);

                        if (getAdultPatientByPassportResult.Success == true)
                        {
                            ItemsList.Clear();

                            foreach (var item in getAdultPatientByPassportResult.AdultPatients)
                            {
                                var newPatientElement = new PatientElement(_mainMenuFrame, item);
                                newPatientElement.MyContext.SetInfo();

                                ItemsList.Add(newPatientElement);
                            }
                        }
                        else
                        {
                            MessageBox.Show(getAdultPatientByPassportResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
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

        private void SearchPatientByName(object parameter)
        {
            _mainMenuFrame.Content = new FindPatientView(_mainMenuFrame);
        }

        private void AddNewPatient(object parameter)
        {
            _mainMenuFrame.Content = new AddNewPatientView(_mainMenuFrame);
        }
    }
}
