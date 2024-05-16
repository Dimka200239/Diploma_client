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

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand UpdateModelCommand { get; }

        public UpdateMathModelVM(Window mainWindow)
        {
            client = new HttpClient();

            configuration = App.Instance.Configuration;

            _baseServerAdress = configuration.GetValue<string>("HttpsBaseServerAdress");
            client.BaseAddress = new Uri(_baseServerAdress);

            var employeeToken = configuration.GetValue<string>("EmployeeToken");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", employeeToken);

            UpdateModelCommand = new RelayCommand(UpdateModel);

            _mainWindow = mainWindow;
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
    }
}
