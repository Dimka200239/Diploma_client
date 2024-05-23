using client.Common;
using client.Model;
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
using System.Windows.Input;

namespace client.ViewModel
{
    public class AddNewEmployeeVM : INotifyPropertyChanged
    {
        private string _login;
        private string _password;
        private string _role;
        private string _lastName;
        private string _name;
        private string _middleName;
        private string _gender;

        private bool _loginIsEnabled;

        private readonly string _baseServerAdress;
        private static HttpClient client;
        private IConfiguration configuration;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand FindCommand { get; }

        public AddNewEmployeeVM()
        {
            client = new HttpClient();

            configuration = App.Instance.Configuration;

            _baseServerAdress = configuration.GetValue<string>("HttpsBaseServerAdress");
            client.BaseAddress = new Uri(_baseServerAdress);

            var employeeToken = configuration.GetValue<string>("EmployeeToken");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", employeeToken);

            Role = "employee";
            Gender = "Женщина";

            AddCommand = new RelayCommand(Add);
            SaveCommand = new RelayCommand(Save);
            FindCommand = new RelayCommand(Find);

            LoginIsEnabled = true;
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

        public string Role
        {
            get { return _role; }
            set
            {
                _role = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Role)));
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName)));
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                _middleName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MiddleName)));
            }
        }

        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Gender)));
            }
        }

        public bool LoginIsEnabled
        {
            get { return _loginIsEnabled; }
            set
            {
                _loginIsEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoginIsEnabled)));
            }
        }

        private async void Add(object parameter)
        {
            if (LoginIsEnabled == false)
            {
                MessageBox.Show("Измените логин, сотрудник с таким логином уже существует!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                LoginIsEnabled = true;
                return;
            }

            if (Login is null || Login.Equals("") ||
                Password is null || Password.Equals("") ||
                LastName is null || LastName.Equals("") ||
                Name is null || Name.Equals(""))
            {
                MessageBox.Show("Не все поля заполнены корректно!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Login != null && Login.Equals("") == false)
            {
                try
                {
                    var getEmployeeResponseBylogin = await client.GetAsync($"/api/auth/getEmployeeByLogin/{Login}");

                    if (getEmployeeResponseBylogin.IsSuccessStatusCode)
                    {
                        string getEmployeeResponseByloginContent = await getEmployeeResponseBylogin.Content.ReadAsStringAsync();
                        var getEmployeeResponseByloginResult = JsonConvert.DeserializeObject<GetEmployeeByLoginResult>(getEmployeeResponseByloginContent);

                        if (getEmployeeResponseByloginResult.Success == true)
                        {
                            MessageBox.Show("Сотрудник с таким логином уже зарегестрирован!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            var registerEmployeeRequest = new RegisterEmployeeRequest
                            {
                                Login = Login,
                                Password = Password,
                                Name = Name,
                                MiddleName = MiddleName,
                                LastName = LastName,
                                Gender = Gender,
                                Role = Role,
                                Token = null
                            };

                            var registerEmployeeResponse = await client.PostAsJsonAsync($"/api/auth/registerEmployee", registerEmployeeRequest);

                            if (registerEmployeeResponse.IsSuccessStatusCode)
                            {
                                string registerEmployeeResponseContent = await registerEmployeeResponse.Content.ReadAsStringAsync();
                                var registerEmployeeResult = JsonConvert.DeserializeObject<AuthResult>(registerEmployeeResponseContent);

                                if (registerEmployeeResult.Success == true)
                                {
                                    MessageBox.Show("Новый сотрудник успешно добавлен", "Успешное добавление", MessageBoxButton.OK, MessageBoxImage.Information);
                                    Login = "";
                                    Password = "";
                                    Role = "employee";
                                    Name = "";
                                    LastName = "";
                                    MiddleName = "";
                                    Gender = "Женщина";
                                }
                                else
                                {
                                    MessageBox.Show(registerEmployeeResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при отправке запроса", "Ошибка соединения", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Нужно ввести логин", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void Save(object parameter)
        {
            if (Login is null || Login.Equals("") ||
                Password is null || Password.Equals("") ||
                LastName is null || LastName.Equals("") ||
                Name is null || Name.Equals(""))
            {
                MessageBox.Show("Не все поля заполнены корректно!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var registerEmployeeRequest = new RegisterEmployeeRequest
                {
                    Login = Login,
                    Password = Password,
                    Role = Role,
                    Name = Name,
                    LastName = LastName,
                    MiddleName = MiddleName,
                    Gender = Gender,
                    Token = null
                };

                var updateEmployeeResponse = await client.PostAsJsonAsync($"/api/auth/updateEmployee", registerEmployeeRequest);

                if (updateEmployeeResponse.IsSuccessStatusCode)
                {
                    string updateEmployeeResponseContent = await updateEmployeeResponse.Content.ReadAsStringAsync();
                    var updateEmployeeResult = JsonConvert.DeserializeObject<UpdateEmployeeResult>(updateEmployeeResponseContent);

                    if (updateEmployeeResult.Success == true)
                    {
                        MessageBox.Show("Данные успешно обновлены", "Успешное обновление", MessageBoxButton.OK, MessageBoxImage.Information);
                        Login = "";
                        Password = "";
                        Role = "employee";
                        Name = "";
                        LastName = "";
                        MiddleName = "";
                        Gender = "Женщина";
                        LoginIsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show(updateEmployeeResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при отправке запроса", "Ошибка соединения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Find(object parameter)
        {
            if (Login != null && Login.Equals("") == false)
            {
                try
                {
                    var getEmployeeResponseBylogin = await client.GetAsync($"/api/auth/getEmployeeByLogin/{Login}");

                    if (getEmployeeResponseBylogin.IsSuccessStatusCode)
                    {
                        string getEmployeeResponseByloginContent = await getEmployeeResponseBylogin.Content.ReadAsStringAsync();
                        var getEmployeeResponseByloginResult = JsonConvert.DeserializeObject<GetEmployeeByLoginResult>(getEmployeeResponseByloginContent);

                        if (getEmployeeResponseByloginResult.Success == true)
                        {
                            LoginIsEnabled = false;

                            Password = "";
                            Role = getEmployeeResponseByloginResult.Employee.Role;
                            Name = getEmployeeResponseByloginResult.Employee.Name;
                            LastName = getEmployeeResponseByloginResult.Employee.LastName;
                            MiddleName = getEmployeeResponseByloginResult.Employee.MiddleName is null ? "" : getEmployeeResponseByloginResult.Employee.MiddleName;
                            Gender = getEmployeeResponseByloginResult.Employee.Gender.Equals("male") ? "Мужчина" : "Женщина";
                        }
                        else
                        {
                            MessageBox.Show(getEmployeeResponseByloginResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при отправке запроса", "Ошибка соединения", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Нужен логин", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
