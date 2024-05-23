using client.Common;
using client.Model;
using client.Results;
using client.View;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace client.ViewModel
{
    public class CounSSZResultVM : INotifyPropertyChanged
    {
        private string _resultText;
        private string _explanationText;
        private string _correlationText;

        private HealthPrediction _healthPrediction;
        private GetLastCorrelationValueResult _getLastCorrelationValueResult;
        private GetPatientWithAddressItemList _patientWithAddressItemList;
        private AnthropometryOfPatient _anthropometryOfPatient;
        private Lifestyle _lifestyle;
        private BloodAnalysis _bloodAnalysis;
        private System.Windows.Controls.Frame _mainMenuFrame;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CreateWordCommand { get; }

        public CounSSZResultVM(HealthPrediction healthPrediction,
            GetLastCorrelationValueResult getLastCorrelationValueResult,
            GetPatientWithAddressItemList patientWithAddressItemList,
            AnthropometryOfPatient anthropometryOfPatient,
            Lifestyle lifestyle,
            BloodAnalysis bloodAnalysis,
            System.Windows.Controls.Frame mainMenuFrame)
        {
            _healthPrediction = healthPrediction;
            _getLastCorrelationValueResult = getLastCorrelationValueResult;
            _patientWithAddressItemList = patientWithAddressItemList;
            _anthropometryOfPatient = anthropometryOfPatient;
            _lifestyle = lifestyle;
            _bloodAnalysis = bloodAnalysis;
            _mainMenuFrame = mainMenuFrame;

            ResultText = _healthPrediction.Prediction.ToString();

            ExplanationText =
                "0 - риск развития ССЗ минимальный (его нет, все замечательно)\r\n" +
                "1 - есть риск развития ССЗ, связанный с повышенным индексом талии/бедра\r\n" +
                "2 - есть риск развития ССЗ, связанный с повышенным коэффициентом атерогенности\r\n" +
                "3 - есть риск развития ССЗ, связанный с образом жизни (курит или выпивает и при это не занимается спортом)\r\n" +
                "4 - есть риск развития ССЗ, связанный с пунктами 1 и 2\r\n" +
                "5 - есть риск развития ССЗ, связанный с пунктами 1 и 3\r\n" +
                "6 - есть риск развития ССЗ, связанный с пунктами 2 и 3\r\n" +
                "7 - есть риск развития ССЗ, связанный с пунктами 1, 2 и 3";

            SetCorrelationResult();

            CreateWordCommand = new RelayCommand(CreateWord);
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

        public string CorrelationText
        {
            get { return _correlationText; }
            set
            {
                _correlationText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CorrelationText)));
            }
        }

        private void SetCorrelationResult()
        {
            if (_getLastCorrelationValueResult == null)
            {
                CorrelationText = "На данный момент нет данных";
            }
            else if (_getLastCorrelationValueResult.CorrelationValue == null)
            {
                CorrelationText = "На данный момент нет данных";
            }
            else
            {
                if (_getLastCorrelationValueResult.CorrelationValue.SmokeCigarettes >= 0.1)
                {
                    CorrelationText += "курение; ";
                }
                if (_getLastCorrelationValueResult.CorrelationValue.DrinkAlcohol >= 0.1)
                {
                    CorrelationText += "употребление алкоголя; ";
                }
                if (_getLastCorrelationValueResult.CorrelationValue.Sport >= 0.1)
                {
                    CorrelationText += "занятие спортом; ";
                }
                if (_getLastCorrelationValueResult.CorrelationValue.AmountOfCholesterol >= 0.1)
                {
                    CorrelationText += "уровень холестерина; ";
                }
                if (_getLastCorrelationValueResult.CorrelationValue.HDL >= 0.1)
                {
                    CorrelationText += "ЛПВП; ";
                }
                if (_getLastCorrelationValueResult.CorrelationValue.LDL >= 0.1)
                {
                    CorrelationText += "ЛПНП; ";
                }
                if (_getLastCorrelationValueResult.CorrelationValue.AtherogenicityCoefficient >= 0.1)
                {
                    CorrelationText += "коэффициент атерогенности; ";
                }
                if (_getLastCorrelationValueResult.CorrelationValue.WHI >= 0.1)
                {
                    CorrelationText += "индекс талии/бедра; ";
                }
            }
        }

        private void CreateWord(object parameter)
        {
            string savePath = GetSavePath();
            if (savePath != null)
            {
                CreateWordDocument(savePath);
            }
        }

        private string GetSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                DefaultExt = "docx",
                AddExtension = true,
                Title = "Сохранение",
                FileName = _patientWithAddressItemList.AdultPatient.GetFullName + "; " + _patientWithAddressItemList.Address.GetFullAddress
            };

            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                return saveFileDialog.FileName;
            }
            return null;
        }

        private void CreateWordDocument(string filepath)
        {
            string predictionText = "";
            string dateTimeNow = DateTime.UtcNow.ToString("dd.MM.yyyy");

            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filepath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                // Add a main document part.
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Add content to the document
                AddParagraph(body, "РЕЗУЛЬТАТЫ ОЦЕНКИ ОБЩЕГО СОСТОЯНИЯ ЗДОРОВЬЯ", true, true, 0, 12, 0, -1.5, -0.5);
                AddParagraph(body, $"Дата выдачи: {dateTimeNow}", false, false, 0, 0, 0, -1.5, 0);
                AddParagraph(body, $"ФИО: {_patientWithAddressItemList.AdultPatient.GetFullName}", false, false, 0, 0, 0, -1.5, 0);
                AddParagraph(body, $"Возраст: {_anthropometryOfPatient.Age}", false, false, 0, 0, 0, -1.5, 0);
                AddParagraph(body, $"Адрес проживания: {_patientWithAddressItemList.Address.GetFullAddress}", false, false, 0, 0, 0, -1.5, 0);
                AddParagraph(body, $"Класс риска развития ССЗ: {_healthPrediction.Prediction}", false, false, 0, 0, 0, -1.5, 0);
                AddParagraph(body, $"Статистически наиболее важные параметры: {CorrelationText}", false, false, 0, 0, 0, -1.5, 0);
                AddParagraph(body, "РАСШИФРОВКА РЕЗУЛЬТАТОВ И ПЕРСОНАЛЬНЫЕ РЕКОМЕНДАЦИИ", true, true, 18, 12, 0, -1.5, -0.5);
                AddParagraph(body, $"Класс риска развития у вас равен {_healthPrediction.Prediction}. Это означает, что {predictionText} ", false, false, 0, 2, 1.25, -1.5, 0);
                AddParagraph(body, $"По статистике для среднестатистического человека наиболее важными параметрами, которые могут влиять на риск развития ССЗ, являются: {CorrelationText}", false, false, 0, 2, 1.25, -1.5, 0);
                AddParagraph(body, "<и т.д. тут ещё будет текст>", false, false, 0, 0, 1.25, -1.5, 0);
            }

            MessageBox.Show("Документ успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            _mainMenuFrame.Content = new AdultPatientProfileView(_mainMenuFrame, _patientWithAddressItemList);
        }

        private void AddParagraph(Body body, string text, bool isBold = false, bool isCentered = false, int spacingBefore = 0, int spacingAfter = 0, double firstLineIndentCm = 0, double leftIndentCm = 0, double rightIndentCm = 0)
        {
            Paragraph paragraph = body.AppendChild(new Paragraph());

            // Настройки параграфа
            ParagraphProperties paragraphProperties = new ParagraphProperties();

            if (isCentered)
            {
                paragraphProperties.AppendChild(new Justification() { Val = JustificationValues.Center });
            }
            else
            {
                paragraphProperties.AppendChild(new Justification() { Val = JustificationValues.Both });
            }

            if (spacingBefore > 0 || spacingAfter > 0)
            {
                paragraphProperties.AppendChild(new SpacingBetweenLines() { Before = (spacingBefore * 20).ToString(), After = (spacingAfter * 20).ToString() });
            }

            // Настройки отступов
            Indentation indentation = new Indentation();

            if (firstLineIndentCm > 0)
            {
                indentation.FirstLine = ((int)(firstLineIndentCm * 567)).ToString(); // 567 TWIPS = 1 cm
            }

            if (leftIndentCm != 0)
            {
                indentation.Left = ((int)(leftIndentCm * 567)).ToString();
            }

            if (rightIndentCm != 0)
            {
                indentation.Right = ((int)(rightIndentCm * 567)).ToString();
            }

            paragraphProperties.AppendChild(indentation);

            paragraphProperties.AppendChild(new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto }); // Одинарный межстрочный интервал

            paragraph.AppendChild(paragraphProperties);

            // Настройки текста
            Run run = paragraph.AppendChild(new Run());
            RunProperties runProperties = new RunProperties();

            runProperties.AppendChild(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" });
            runProperties.AppendChild(new FontSize() { Val = "28" }); // 14 points = 28 half-points

            if (isBold)
            {
                runProperties.AppendChild(new Bold());
            }

            run.AppendChild(runProperties);
            run.AppendChild(new Text(text));
        }
    }
}
