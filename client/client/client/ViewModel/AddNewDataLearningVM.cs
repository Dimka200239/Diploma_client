using client.Common;
using client.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System;
using client.Requests;
using System.Net.Http.Json;
using System.Windows;
using client.Results;
using Newtonsoft.Json;
using System.Collections.Generic;
using client.View;

namespace client.ViewModel
{
    public class AddNewDataLearningVM : INotifyPropertyChanged
    {
        ObservableCollection<DataForFutureLearning> _dataForFutureLearningList;

        private readonly string _baseServerAdress;
        private static HttpClient client;
        private IConfiguration configuration;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddCommand { get; }

        public AddNewDataLearningVM()
        {
            client = new HttpClient();

            configuration = App.Instance.Configuration;

            _baseServerAdress = configuration.GetValue<string>("HttpsBaseServerAdress");
            client.BaseAddress = new Uri(_baseServerAdress);

            var employeeToken = configuration.GetValue<string>("EmployeeToken");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", employeeToken);

            AddCommand = new RelayCommand(Add);

            LoadData();
        }

        public ObservableCollection<DataForFutureLearning> DataForFutureLearningList
        {
            get { return _dataForFutureLearningList; }
            set
            {
                _dataForFutureLearningList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DataForFutureLearningList)));
            }
        }

        private async void LoadData()
        {
            DataForFutureLearningList = new ObservableCollection<DataForFutureLearning>();
            
            try
            {
                var getAllDataForFutureLearningResponse = await client.GetAsync($"/api/dataForFutureLearning/getAllDataForFutureLearning");

                if (getAllDataForFutureLearningResponse.IsSuccessStatusCode)
                {
                    string getAllDataForFutureLearningResponseContent = await getAllDataForFutureLearningResponse.Content.ReadAsStringAsync();
                    var getAllDataForFutureLearningResult = JsonConvert.DeserializeObject<GetAllDataForFutureLearningResult>(getAllDataForFutureLearningResponseContent);

                    if (getAllDataForFutureLearningResult.Success == true)
                    {
                        foreach (var item in getAllDataForFutureLearningResult.DataForFutureLearnings)
                        {
                            DataForFutureLearningList.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show(getAllDataForFutureLearningResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void Add(object parameter)
        {
            try
            {
                var dateForForecastingList = new List<DateForForecasting>();

                foreach (var item in _dataForFutureLearningList)
                {
                    var newDateForForecasting = new DateForForecasting()
                    {
                        Gender = item.Gender,
                        Age = item.Age,
                        SmokeCigarettes = item.SmokeCigarettes,
                        DrinkAlcohol = item.DrinkAlcohol,
                        Sport = item.Sport,
                        AmountOfCholesterol = item.AmountOfCholesterol,
                        HDL = item.HDL,
                        LDL = item.LDL,
                        AtherogenicityCoefficient = item.AtherogenicityCoefficient,
                        WHI = item.WHI,
                        HasCVD = item.HasCVD
                    };

                    dateForForecastingList.Add(newDateForForecasting);
                }

                var addDateForForecastingRequest = new AddDateForForecastingRequest
                {
                    DateForForecastingList = dateForForecastingList
                };

                var addDateForForecastingResponse = await client.PostAsJsonAsync($"/api/dateForForecasting/addDateForForecasting", addDateForForecastingRequest);

                if (addDateForForecastingResponse.IsSuccessStatusCode)
                {
                    string addDateForForecastingResponseContent = await addDateForForecastingResponse.Content.ReadAsStringAsync();
                    var addDateForForecastingResult = JsonConvert.DeserializeObject<AddDateForForecastingResult>(addDateForForecastingResponseContent);

                    if (addDateForForecastingResult.Success == true)
                    {
                        var clearAllDataForFutureLearningResponse = await client.DeleteAsync($"/api/dataForFutureLearning/clearAllDataForFutureLearning");

                        if (clearAllDataForFutureLearningResponse.IsSuccessStatusCode)
                        {
                            string clearAllDataForFutureLearningResponseContent = await clearAllDataForFutureLearningResponse.Content.ReadAsStringAsync();
                            var clearAllDataForFutureLearningResult = JsonConvert.DeserializeObject<ClearAllDataForFutureLearningResult>(clearAllDataForFutureLearningResponseContent);

                            if (clearAllDataForFutureLearningResult.Success == true)
                            {
                                DataForFutureLearningList.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Ошибка удаления данных из промежуточной таблицы", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }

                        MessageBox.Show("Новые данные для переобучения модели добавлены", "Успешное добавление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(addDateForForecastingResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
