using client.Model;
using client.Results;
using System.ComponentModel;
using System.Windows.Controls;

namespace client.ViewModel
{
    public class CounSSZResultVM : INotifyPropertyChanged
    {
        private string _resultText;
        private string _explanationText;

        private HealthPrediction _healthPrediction;

        public event PropertyChangedEventHandler PropertyChanged;

        public CounSSZResultVM(HealthPrediction healthPrediction)
        {
            _healthPrediction = healthPrediction;

            ResultText = "Класс риска развития ССЗ : " + _healthPrediction.Prediction.ToString();

            _explanationText =
                "-- 0 - риск развития ССЗ минимальный (его нет, все замечательно)\r\n" +
                "-- 1 - есть риск развития ССЗ, связанный с повышенным индексом талии/бедра\r\n" +
                "-- 2 - есть риск развития ССЗ, связанный с повышенным коэффициентом атерогенности\r\n" +
                "-- 3 - есть риск развития ССЗ, связанный с образом жизни (курит или выпивает и при это не занимается спортом)\r\n" +
                "-- 4 - есть риск развития ССЗ, связанный с пунктами 1 и 2\r\n" +
                "-- 5 - есть риск развития ССЗ, связанный с пунктами 1 и 3\r\n" +
                "-- 6 - есть риск развития ССЗ, связанный с пунктами 2 и 3\r\n" +
                "-- 7 - есть риск развития ССЗ, связанный с пунктами 1, 2 и 3";
        }

        public string ResultText
        {
            get { return _resultText; }
            set
            {
                _resultText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ResultText)));
            }
        }

        public string ExplanationText
        {
            get { return _explanationText; }
            set
            {
                _explanationText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExplanationText)));
            }
        }
    }
}
