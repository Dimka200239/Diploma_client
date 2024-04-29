using client.Common;
using client.Model;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace client.ViewModel
{
    public class MainEmployeeMenuVM : INotifyPropertyChanged
    {
        private int _menuButtonWidth;
        private string _menuButtonSourceImg;
        private string _menuButtonHorizontalAlignment;
        private int _menuButtonImgWidth;
        private bool _menuButtonIsOpen;
        private string _menuButtonsIsEnabled;

        private string _findPatientText;
        private string _dataTransferText;
        private string _colculationText;
        private string _findConlusionText;
        private string _settingsText;

        private Employee _employee { get; set; }
        private Frame _mainFrame;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand MenuButtonClickCommand { get; }

        public MainEmployeeMenuVM(Employee employee, Frame mainFrame)
        {
            _employee = employee;
            _mainFrame = mainFrame;

            MenuButtonWidth = 30;
            MenuButtonSourceImg = "/View/Img/employeeMenu.png";
            MenuButtonHorizontalAlignment = "Right";
            _menuButtonIsOpen = false;
            _menuButtonsIsEnabled = "Collapsed";

            MenuButtonClickCommand = new RelayCommand(MenuButtonClick);
        }

        public int MenuButtonWidth
        {
            get { return _menuButtonWidth; }
            set
            {
                _menuButtonWidth = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuButtonWidth)));
                MenuButtonImgWidth = _menuButtonWidth - 5;
            }
        }

        public string MenuButtonSourceImg
        {
            get { return _menuButtonSourceImg; }
            set
            {
                _menuButtonSourceImg = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuButtonSourceImg)));
            }
        }

        public string MenuButtonHorizontalAlignment
        {
            get { return _menuButtonHorizontalAlignment; }
            set
            {
                _menuButtonHorizontalAlignment = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuButtonHorizontalAlignment)));
            }
        }
        
        public int MenuButtonImgWidth
        {
            get { return _menuButtonImgWidth; }
            set
            {
                _menuButtonImgWidth = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuButtonImgWidth)));
            }
        }

        public string FindPatientText
        {
            get { return _findPatientText; }
            set
            {
                _findPatientText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FindPatientText)));
            }
        }

        public string DataTransferText
        {
            get { return _dataTransferText; }
            set
            {
                _dataTransferText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DataTransferText)));
            }
        }

        public string ColculationText
        {
            get { return _colculationText; }
            set
            {
                _colculationText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColculationText)));
            }
        }

        public string FindConlusionText
        {
            get { return _findConlusionText; }
            set
            {
                _findConlusionText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FindConlusionText)));
            }
        }

        public string SettingsText
        {
            get { return _settingsText; }
            set
            {
                _settingsText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SettingsText)));
            }
        }

        public string MenuButtonsIsEnabled
        {
            get { return _menuButtonsIsEnabled; }
            set
            {
                _menuButtonsIsEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuButtonsIsEnabled)));
            }
        }

        private void MenuButtonClick(object parameter)
        {
            if (_menuButtonIsOpen == false)
            {
                MenuButtonWidth = 200;
                MenuButtonSourceImg = "/View/Img/getBack.png";
                MenuButtonHorizontalAlignment = "Center";
                _menuButtonIsOpen = true;
                FindPatientText = "Поиск пациентов";
                DataTransferText = "Перенос данных о пациенте";
                ColculationText = "Расчёт заключения";
                FindConlusionText = "Поиск заключений";
                SettingsText = "Настройки";
                MenuButtonsIsEnabled = "Visible";
            }
            else
            {
                MenuButtonWidth = 30;
                MenuButtonSourceImg = "/View/Img/employeeMenu.png";
                MenuButtonHorizontalAlignment = "Right";
                _menuButtonIsOpen = false;
                FindPatientText = "";
                DataTransferText = "";
                ColculationText = "";
                FindConlusionText = "";
                SettingsText = "";
                MenuButtonsIsEnabled = "Collapsed";
            }
        }
    }
}
