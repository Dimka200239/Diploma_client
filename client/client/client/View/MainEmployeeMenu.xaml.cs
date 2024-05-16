﻿using client.Model;
using client.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace client.View
{
    /// <summary>
    /// Логика взаимодействия для MainEmployeeMenu.xaml
    /// </summary>
    public partial class MainEmployeeMenu : Page
    {
        private MainEmployeeMenuVM _myContex;

        public MainEmployeeMenu(Employee employee, Frame MainFrame, Window mainWindow)
        {
            InitializeComponent();
            var nav = NavigationService.GetNavigationService(MainMenuFrame);
            _myContex = new MainEmployeeMenuVM(employee, MainFrame, MainMenuFrame, mainWindow);
            DataContext = _myContex;
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_myContex.IsDimmed == true)
            {
                _myContex.CloseSection();
            }
        }
    }
}
