using client.Common;
using client.Requests;
using client.Results;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace client.ViewModel
{
    public class UserLoginAndRegistrationPageVM : INotifyPropertyChanged
    {
        private string _login;
        private string _password;
        private readonly Frame MainFrame;
        private readonly string _baseServerAdress;
        private static readonly HttpClient client = new HttpClient();

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoginCommand { get; }

        public UserLoginAndRegistrationPageVM(Frame MainFrame)
        {
            this.MainFrame = MainFrame;
            LoginCommand = new RelayCommand(Authorize);
            var appSettings = App.Instance.Configuration;
            _baseServerAdress = appSettings.GetValue<string>("HttpsBaseServerAdress");
            client.BaseAddress = new Uri(_baseServerAdress);
        }

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Login)));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        private async void Authorize(object parameter)
        {
            var loginRequest = new LoginRequest
            {
                Login = this.Login,
                Password = this.Password
            };

            try
            {
                var response = await client.PostAsJsonAsync($"/api/auth/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<AuthResult>(responseContent);

                    if (result.Success == true)
                    {
                        //переход к окну профиля
                    }
                    else
                    {
                        MessageBox.Show(result.Errors[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при попытке подключения к серверу...", "Ошибка соединения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
