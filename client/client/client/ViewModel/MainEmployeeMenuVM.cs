﻿using client.Common;
using client.Model;
using client.View;
using System.ComponentModel;
using System.Windows;
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
        private string _settingsText;
        private string _exitText;

        private bool _isDimmed;

        private Employee _employee { get; set; }
        private Frame _mainFrame;
        private Window _mainWindow;
        private Frame _mainMenuFrame;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand MenuButtonClickCommand { get; }
        public ICommand OpenFindPatientWindowCommand { get; }
        public ICommand SettingsClickCommand { get; }
        public ICommand ExitClickCommand { get; }

        public MainEmployeeMenuVM(Employee employee, Frame mainFrame, Frame mainMenuFrame, Window mainWindow)
        {
            _employee = employee;
            _mainFrame = mainFrame;
            _mainWindow = mainWindow;
            _mainMenuFrame = mainMenuFrame;

            MenuButtonWidth = 30;
            MenuButtonSourceImg = "/View/Img/employeeMenu.png";
            MenuButtonHorizontalAlignment = "Right";
            _menuButtonIsOpen = false;
            _menuButtonsIsEnabled = "Collapsed";

            MenuButtonClickCommand = new RelayCommand(MenuButtonClick);
            OpenFindPatientWindowCommand = new RelayCommand(OpenFindPatientWindow);
            SettingsClickCommand = new RelayCommand(SettingsClick);
            ExitClickCommand = new RelayCommand(ExitClick);

            _mainMenuFrame.Content = new FindPatientView(_mainMenuFrame);
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

        public string SettingsText
        {
            get { return _settingsText; }
            set
            {
                _settingsText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SettingsText)));
            }
        }

        public string ExitText
        {
            get { return _exitText; }
            set
            {
                _exitText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExitText)));
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

        public bool IsDimmed
        {
            get { return _isDimmed; }
            set
            {
                _isDimmed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDimmed)));
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
                FindPatientText = "Найти пациента";
                SettingsText = "Настройки";
                ExitText = "Выйти";
                MenuButtonsIsEnabled = "Visible";
                IsDimmed = !IsDimmed;
            }
            else
            {
                CloseSection();
            }
        }

        private void OpenFindPatientWindow(object parameter)
        {
            _mainMenuFrame.Content = new FindPatientView(_mainMenuFrame);
            CloseSection();
        }

        private void SettingsClick(object parameter)
        {
            CloseSection();
        }

        private void ExitClick(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите выйти?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                CloseSection();
                return;
            }
            else
            {
                _mainFrame.Content = new LoginView(_mainFrame, _mainWindow);
            }
        }

        public void CloseSection()
        {
            MenuButtonWidth = 30;
            MenuButtonSourceImg = "/View/Img/employeeMenu.png";
            MenuButtonHorizontalAlignment = "Right";
            _menuButtonIsOpen = false;
            FindPatientText = "";
            SettingsText = "";
            ExitText = "";
            MenuButtonsIsEnabled = "Collapsed";
            IsDimmed = !IsDimmed;
        }
    }
}
