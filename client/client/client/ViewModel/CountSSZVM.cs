using client.Common;
using client.Model;
using client.Requests;
using client.Results;
using client.View;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace client.ViewModel
{
    public class CountSSZVM : INotifyPropertyChanged
    {
        private CultureInfo culture = CultureInfo.GetCultureInfo("ru-RU");

        private Frame _mainMenuFrame;
        private GetPatientWithAddressItemList _patientWithAddressItemList;
        private AnthropometryOfPatient _anthropometryOfPatient;
        private Lifestyle _lifestyle;

        private readonly string _baseServerAdress;
        private static HttpClient client;
        private IConfiguration configuration;

        private string _height;
        private string _weight;
        private string _waist;
        private string _hip;

        private string _smoke;
        private string _drink;
        private string _sport;

        private string _cholesterol;
        private string _triglycerides;
        private string _LDL;

        private string _HDL;
        private string _VLDL;
        private string _atherogenicCoefficient;
        private string _BMI;
        private string _WHI;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GetBackCommand { get; }
        public ICommand CountCommand { get; }
        public ICommand SaveCommand { get; }

        public CountSSZVM(Frame mainMenuFrame, GetPatientWithAddressItemList patientWithAddressItemList,
            AnthropometryOfPatient anthropometryOfPatient, Lifestyle lifestyle)
        {
            client = new HttpClient();

            configuration = App.Instance.Configuration;

            _baseServerAdress = configuration.GetValue<string>("HttpsBaseServerAdress");
            client.BaseAddress = new Uri(_baseServerAdress);

            var employeeToken = configuration.GetValue<string>("EmployeeToken");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", employeeToken);

            _mainMenuFrame = mainMenuFrame;
            _patientWithAddressItemList = patientWithAddressItemList;
            _anthropometryOfPatient = anthropometryOfPatient;
            _lifestyle = lifestyle;

            CountCommand = new RelayCommand(Count);
            GetBackCommand = new RelayCommand(GetBack);
            SaveCommand = new RelayCommand(Save);
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

        public string Cholesterol
        {
            get { return _cholesterol; }
            set
            {
                _cholesterol = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cholesterol)));
            }
        }

        public string Triglycerides
        {
            get { return _triglycerides; }
            set
            {
                _triglycerides = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Triglycerides)));
            }
        }

        public string LDL
        {
            get { return _LDL; }
            set
            {
                _LDL = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LDL)));
            }
        }

        public string HDL
        {
            get { return _HDL; }
            set
            {
                _HDL = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HDL)));
            }
        }

        public string VLDL
        {
            get { return _VLDL; }
            set
            {
                _VLDL = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VLDL)));
            }
        }

        public string AtherogenicCoefficient
        {
            get { return _atherogenicCoefficient; }
            set
            {
                _atherogenicCoefficient = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AtherogenicCoefficient)));
            }
        }

        public string BMI
        {
            get { return _BMI; }
            set
            {
                _BMI = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BMI)));
            }
        }

        public string WHI
        {
            get { return _WHI; }
            set
            {
                _WHI = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WHI)));
            }
        }

        private void GetBack(object parameter)
        {
            _mainMenuFrame.Content = new AdultPatientProfileView(_mainMenuFrame, _patientWithAddressItemList);
        }

        private void Count(object parameter)
        {
            HDL = Math.Round(float.Parse(Cholesterol, culture) - (float.Parse(LDL, culture) + (float.Parse(Triglycerides, culture) / 2.2)), 2).ToString();
            VLDL = Math.Round(float.Parse(Triglycerides, culture) / 5, 2).ToString();
            AtherogenicCoefficient = Math.Round((float.Parse(VLDL, culture) + float.Parse(LDL, culture)) / float.Parse(HDL, culture), 2).ToString();
            BMI = Math.Round(float.Parse(Weight, culture) / Math.Pow(float.Parse(Height, culture) / 100, 2), 2).ToString();
            WHI = Math.Round(float.Parse(Waist, culture) / float.Parse(Hip, culture), 2).ToString();
        }

        private async void Save(object parameter)
        {
            if (Cholesterol is null || Cholesterol.Equals("") ||
                Triglycerides is null || Triglycerides.Equals("") ||
                LDL is null || LDL.Equals("") ||
                HDL is null || HDL.Equals("") ||
                VLDL is null || VLDL.Equals("") ||
                AtherogenicCoefficient is null || AtherogenicCoefficient.Equals("") ||
                BMI is null || BMI.Equals("") ||
                WHI is null || WHI.Equals(""))
            {
                MessageBox.Show("Вы не заполнили все необходимые поля!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var createBloodAnalysisRequest = new BloodAnalysisRequest
                {
                    PatientId = _patientWithAddressItemList.AdultPatient.Id,
                    Role = _patientWithAddressItemList.AdultPatient.Role,
                    AmountOfCholesterol = Math.Round(float.Parse(Cholesterol, culture), 2),
                    HDL = Math.Round(float.Parse(HDL, culture), 2),
                    LDL = Math.Round(float.Parse(LDL, culture), 2),
                    VLDL = Math.Round(float.Parse(VLDL, culture), 2),
                    AtherogenicityCoefficient = Math.Round(float.Parse(AtherogenicCoefficient, culture), 2),
                    BMI = Math.Round(float.Parse(BMI, culture), 2),
                    WHI = Math.Round(float.Parse(WHI, culture), 2),
                };

                var createBloodAnalysisResponse = await client.PostAsJsonAsync($"/api/bloodAnalysis/createBloodAnalysis", createBloodAnalysisRequest);

                if (createBloodAnalysisResponse.IsSuccessStatusCode)
                {
                    string createBloodAnalysisResponseContent = await createBloodAnalysisResponse.Content.ReadAsStringAsync();
                    var createBloodAnalysisResult = JsonConvert.DeserializeObject<CreateBloodAnalysisResult>(createBloodAnalysisResponseContent);

                    if (createBloodAnalysisResult.Success == true)
                    {
                        MessageBox.Show("Новый анализ успешно добавлен!", "Успешное добавление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(createBloodAnalysisResult.Errors[0], "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void SetInfo()
        {
            Height = _anthropometryOfPatient.Height.ToString();
            Weight = _anthropometryOfPatient.Weight.ToString();
            Waist = _anthropometryOfPatient.Waist.ToString();
            Hip = _anthropometryOfPatient.Hip.ToString();

            Smoke = _lifestyle.SmokeCigarettes == true ? "Да" : "Нет";
            Drink = _lifestyle.DrinkAlcohol == true ? "Да" : "Нет";
            Sport = _lifestyle.Sport == true ? "Да" : "Нет";
        }
    }
}
