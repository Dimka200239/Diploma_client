using client.Common;
using client.Requests;
using client.Results;
using client.View;
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
        private bool _rememberMe;
        private readonly Frame _mainFrame;
        private readonly string _baseServerAdress;
        private static HttpClient client;
        private IConfiguration configuration;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoginCommand { get; }

        public UserLoginAndRegistrationPageVM(Frame mainFrame)
        {
            client = new HttpClient();

            configuration = App.Instance.Configuration;
            _mainFrame = mainFrame;
            LoginCommand = new RelayCommand(Authorize);
            _baseServerAdress = configuration.GetValue<string>("HttpsBaseServerAdress");
            _login = configuration.GetValue<string>("LastEmployeeLogin");
            _password = configuration.GetValue<string>("LastEmployeePassword");
            client.BaseAddress = new Uri(_baseServerAdress);
            RememberMe = true;
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

        public bool RememberMe
        {
            get { return _rememberMe; }
            set
            {
                _rememberMe = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RememberMe)));
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
                        var writeTokenToAppSettingsClass = new WriteTokenToAppSettingsClass();
                        writeTokenToAppSettingsClass.WriteToken(result.Token);

                        var employeeToken = configuration.GetValue<string>("EmployeeToken");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", employeeToken);

                        var getEmployeeResponseBylogin = await client.GetAsync($"/api/auth/getEmployeeByLogin/{result.Login}");

                        if (getEmployeeResponseBylogin.IsSuccessStatusCode)
                        {
                            string getEmployeeResponseByloginContent = await getEmployeeResponseBylogin.Content.ReadAsStringAsync();
                            var getEmployeeResponseByloginResult = JsonConvert.DeserializeObject<GetEmployeeByLoginResult>(responseContent);

                            if (getEmployeeResponseByloginResult.Success == true)
                            {
                                if (_rememberMe == true)
                                {
                                    writeTokenToAppSettingsClass.WriteLoginAndPassword(_login, _password);
                                }
                                else
                                {
                                    writeTokenToAppSettingsClass.ClearLoginAndPassWord();
                                }

                                _mainFrame.Content = new MainEmployeeMenu(getEmployeeResponseByloginResult.Employee, _mainFrame);
                            }
                            else
                            {
                                MessageBox.Show(getEmployeeResponseByloginResult.Errors[0]);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при попытке подключения к серверу...", "Ошибка соединения", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
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
