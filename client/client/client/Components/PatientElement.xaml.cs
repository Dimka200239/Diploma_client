using client.Model;
using client.Results;
using client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
namespace client.Components
{
    /// <summary>
    /// Логика взаимодействия для PatientElement.xaml
    /// </summary>
    public partial class PatientElement : UserControl
    {
        private PatientElementVM _myContex;

        public PatientElement(Frame mainMenuFrame, GetPatientWithAddressItemList patientWithAddressItemList)
        {
            InitializeComponent();
            _myContex = new PatientElementVM(mainMenuFrame, patientWithAddressItemList);
            DataContext = _myContex;
        }

        public PatientElement()
        {
            InitializeComponent();
        }

        public PatientElementVM MyContext
        {
            get
            {
                return _myContex;
            }
            private set
            {
                _myContex = value;
            }
        }
    }
}
