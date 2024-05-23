using client.Common;
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
using System.Windows.Input;

namespace client.ViewModel
{
    public class UpdateMathModelVM : INotifyPropertyChanged
    {
        private readonly string _baseServerAdress;
        private static HttpClient client;
        private IConfiguration configuration;

        private Window _mainWindow;

        private string _mLDate;
        private string _mLCount;
        private string _corelationDate;
        private string _corelationCount;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand UpdateModelCommand { get; }
        public ICommand UpdateCorelationCommand { get; }

        public UpdateMathModelVM(Window mainWindow)
        {
            client = new HttpClient();

            configuration = App.Instance.Configuration;

            _baseServerAdress = configuration.GetValue<string>("HttpsBaseServerAdress");
            client.BaseAddress = new Uri(_baseServerAdress);

            var employeeToken = configuration.GetValue<string>("EmployeeToken");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", employeeToken);

            UpdateModelCommand = new RelayCommand(UpdateModel);
            UpdateCorelationCommand = new RelayCommand(UpdateCorelation);

            _mainWindow = mainWindow;

            LoadLastVersion();
        }

        public string MLDate
        {
            get { return _mLDate; }
            set
            {
                _mLDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MLDate)));
            }
        }

        public string MLCount
        {
            get { return _mLCount; }
            set
            {
                _mLCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MLCount)));
            }
        }

        public string CorelationDate
        {
            get { return _corelationDate; }
            set
            {
                _corelationDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CorelationDate)));
            }
        }

        public string CorelationCount
        {
            get { return _corelationCount; }
            set
            {
                _corelationCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CorelationCount)));
            }
        }

        private async void UpdateModel(object parameter)
        {
            try
            {
                var updateModelResponse = await client.PostAsJsonAsync($"/api/machineLearningModel/updateMachineLearningModel", true);

                if (updateModelResponse.IsSuccessStatusCode)
                {
                    string updateModelResponseContent = await updateModelResponse.Content.ReadAsStringAsync();
                    var updateModelResult = JsonConvert.DeserializeObject<UpdateMachineLearningModelResult>(updateModelResponseContent);

                    if (updateModelResult.Success == true)
                    {
                        MessageBox.Show("Модель успешно переобучена", "Успешное переобучение", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(updateModelResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void UpdateCorelation(object parameter)
        {
            try
            {
                var updateCorelationResponse = await client.PostAsJsonAsync($"/api/correlation/getCorrelation", true);

                if (updateCorelationResponse.IsSuccessStatusCode)
                {
                    string updateCorelationResponseContent = await updateCorelationResponse.Content.ReadAsStringAsync();
                    var updateCorelationResult = JsonConvert.DeserializeObject<GetCorrelationResult>(updateCorelationResponseContent);

                    if (updateCorelationResult.Success == true)
                    {
                        MessageBox.Show("Статистика успешно обновлена", "Успешное переобучение", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(updateCorelationResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void LoadLastVersion()
        {
            try
            {
                var getLastCorrelationValueResponse = await client.GetAsync($"/api/correlation/getLastCorrelationValue");

                if (getLastCorrelationValueResponse.IsSuccessStatusCode)
                {
                    string getLastCorrelationValueResponseContent = await getLastCorrelationValueResponse.Content.ReadAsStringAsync();
                    var getLastCorrelationValueResult = JsonConvert.DeserializeObject<GetLastCorrelationValueResult>(getLastCorrelationValueResponseContent);

                    if (getLastCorrelationValueResult.Success == true)
                    {
                        CorelationDate = getLastCorrelationValueResult.CorrelationValue.CreatedDate.ToString("dd.MM.yyyy");
                        CorelationCount = getLastCorrelationValueResult.CorrelationValue.CountOfData.ToString();
                    }
                    else
                    {
                        MessageBox.Show(getLastCorrelationValueResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                var getLastMLResponse = await client.GetAsync($"/api/machineLearningModel/getLastVersion");

                if (getLastMLResponse.IsSuccessStatusCode)
                {
                    string getLastMLResponseContent = await getLastMLResponse.Content.ReadAsStringAsync();
                    var getLastMLResult = JsonConvert.DeserializeObject<GetLastVersionResult>(getLastMLResponseContent);

                    if (getLastMLResult.Success == true)
                    {
                        MLDate = getLastMLResult.MachineLearningModel.CreatedDate.ToString("dd.MM.yyyy");
                        MLCount = getLastMLResult.MachineLearningModel.CountOfData.ToString();
                    }
                    else
                    {
                        MessageBox.Show(getLastMLResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
