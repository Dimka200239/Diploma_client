using client.Model;
using client.Results;
using client.ViewModel;
using System.Windows.Controls;

namespace client.View
{
    /// <summary>
    /// Логика взаимодействия для CounSSZResultView.xaml
    /// </summary>
    public partial class CounSSZResultView : Page
    {
        public CounSSZResultView(HealthPrediction healthPrediction,
            GetLastCorrelationValueResult getLastCorrelationValueResult,
            GetPatientWithAddressItemList patientWithAddressItemList,
            AnthropometryOfPatient anthropometryOfPatient,
            Lifestyle lifestyle,
            BloodAnalysis bloodAnalysis,
            Frame mainMenuFrame)
        {
            InitializeComponent();
            DataContext = new CounSSZResultVM(healthPrediction,
                getLastCorrelationValueResult,
                patientWithAddressItemList,
                anthropometryOfPatient,
                lifestyle,
                bloodAnalysis,
                mainMenuFrame);
        }
    }
}
