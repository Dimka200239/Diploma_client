using client.Common;
using client.Model;
using client.Results;
using client.View;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
            string predictionText = GetInfoByClass(_healthPrediction.Prediction);
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
                var hdlResult = GetHDLValue(_anthropometryOfPatient.Age, _patientWithAddressItemList.AdultPatient.Gender);
                var ldlResult = GetLDLValue(_anthropometryOfPatient.Age, _patientWithAddressItemList.AdultPatient.Gender);
                var caResult = GetCAValue(_anthropometryOfPatient.Age, _patientWithAddressItemList.AdultPatient.Gender);
                var whilResult = GetWHIValue(_patientWithAddressItemList.AdultPatient.Gender);
                var strHdlResult = $"Липопротеиды высокой плотности: {_bloodAnalysis.HDL} (норма: от {hdlResult.Lower} до {hdlResult.Upper})";
                var strLdlResult = $"Липопротеиды низкой плотности: {_bloodAnalysis.LDL} (норма: от {ldlResult.Lower} до {ldlResult.Upper})";
                var strCAResult = $"Коэффициент атерогенности: {_bloodAnalysis.AtherogenicityCoefficient} (норма: от {caResult.Lower} до {caResult.Upper})";
                var strWHIResult = $"Индекс талии/бедра: {_bloodAnalysis.WHI} (норма: от {whilResult.Lower} до {whilResult.Upper})";
                AddParagraph(body, $"{strHdlResult}", false, false, 0, 0, 0, -1.5, 0);
                AddParagraph(body, $"{strLdlResult}", false, false, 0, 0, 0, -1.5, 0);
                AddParagraph(body, $"{strCAResult}", false, false, 0, 0, 0, -1.5, 0);
                AddParagraph(body, $"{strWHIResult}", false, false, 0, 0, 0, -1.5, 0);
                AddParagraph(body, $"Класс риска развития ССЗ: {_healthPrediction.Prediction}", false, false, 0, 0, 0, -1.5, 0);
                AddParagraph(body, "РАСШИФРОВКА РЕЗУЛЬТАТОВ", true, true, 18, 12, 0, -1.5, -0.5);
                AddParagraph(body, $"Класс риска развития у вас равен {_healthPrediction.Prediction}. Это означает, что {predictionText} ", false, false, 0, 2, 1.25, -1.5, 0);
                AddParagraph(body, $"По статистике для среднестатистического человека наиболее важными параметрами, " +
                    $"которые в большей степени влияют на риск развития ССЗ, являются: {CorrelationText}, поэтому советуем в первую " +
                    $"очередь следить за этими показателями.", false, false, 0, 2, 1.25, -1.5, 0);
                AddParagraph(body, "СОВЕТЫ И РЕКОМЕНДАЦИИ", true, true, 18, 12, 0, -1.5, -0.5);
                AddParagraph(body, $"Даже если всё плохо, то никогда не поздно всё исправить! {GetRecomendateByClass(_healthPrediction.Prediction)}", false, false, 0, 2, 1.25, -1.5, 0);
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

        private string GetInfoByClass(float prediction)
        {
            var result = "";

            if (prediction == 0)
            {
                result = "у вас минимальный риск развития сердечно-сосудистых заболеваний.";
            }
            if (prediction == 1)
            {
                result = "у вас есть риск развития сердечно-сосудистых заболеваний, связанный с повышенным индексом талии/бедра. " +
                    "Один из важных показателей при оценке избыточного веса — соотношение окружности талии к окружности бедер. " +
                    "Особенно опасен жир, наросший на животе и внутри. Абдоминальный жир скапливается вокруг внутренних органов, " +
                    "например, печени, мешая их работе и способствуя нарушению обмена веществ. Также чем больше жира на животе, " +
                    "тем больше вероятность развития сердечно-сосудистых проблем, так как жировая ткань в области живота участвует в " +
                    "обмене веществ организма и может способствовать образованию отложений в кровеносных сосудах.";
            }
            if (prediction == 2)
            {
                result = "у вас есть риск развития сердечно-сосудистых заболеваний, связанный с повышенным коэффицентом атерогенности. " +
                    "Коэффициент атерогенности – отношение \"плохого\" холестерола к \"хорошему\", характеризующее риск развития " +
                    "сердечно-сосудистых заболеваний. Чем больше значение \"плохого\" холестерола и меньше знаечние " +
                    "\"хорошего\" холестерола, тем больше риск развития ССЗ.";
            }
            if (prediction == 3)
            {
                result = "у вас есть риск развития сердечно-сосудистых заболеваний, связанный с вашим образом жизни. " +
                    "При выкуривании одной сигареты сердце человека начинает работать в ускоренном ритме, который " +
                    "сохраняется на протяжении 15 минут. Хватает всего нескольких затяжек, чтобы артериальное давление" +
                    "увеличилось на 5%, пульс участился на 14%. Влияние курения на сердечно-сосудистую систему проявляется " +
                    "также в том, что повышается свёртываемость и вязкость крови, изменения адгезивных свойств тромбоцитов," +
                    "а это способствует усилению тромбообразования. С курением сигарет связана не только ишемическая" +
                    "болезнь сердца. В ещё большей степени оно является причиной заболеваний периферических сосудов, " +
                    "в частности, эндартериита нижних конечностей (перемежающая хромота или облитерирующий эндартериит). " +
                    "Попавший в кровь этанол на протяжении 5-7 часов заставляет сердце работать с повышенной нагрузкой. " +
                    "Пагубное влияние алкоголя на сердечно-сосудистую систему связано с учащением сердцебиения, " +
                    "повышением кровяного давления и нарушением кровообращения. На сосуды алкоголь действует двухфазно: " +
                    "сначала сильно расширяет, а потом сужает. Это приводит к чрезмерным нагрузкам на сердце и нарушению его работы.";
            }
            if (prediction == 4)
            {
                result = "у вас есть риск развития сердечно-сосудистых заболеваний, связанный с повышенным индексом талии/бедра и " +
                    "повышенным коэффицентом атерогенности. " +
                    "Один из важных показателей при оценке избыточного веса — соотношение окружности талии к окружности бедер. " +
                    "Особенно опасен жир, наросший на животе и внутри. Абдоминальный жир скапливается вокруг внутренних органов, " +
                    "например, печени, мешая их работе и способствуя нарушению обмена веществ. Также чем больше жира на животе, " +
                    "тем больше вероятность развития сердечно-сосудистых проблем, так как жировая ткань в области живота участвует в " +
                    "обмене веществ организма и может способствовать образованию отложений в кровеносных сосудах. " +
                    "Коэффициент атерогенности – отношение \"плохого\" холестерола к \"хорошему\", характеризующее риск развития " +
                    "сердечно-сосудистых заболеваний. Чем больше значение \"плохого\" холестерола и меньше знаечние " +
                    "\"хорошего\" холестерола, тем больше риск развития ССЗ.";
            }
            if (prediction == 5)
            {
                result = "у вас есть риск развития сердечно-сосудистых заболеваний, связанный с повышенным индексом талии/бедра и " +
                    "вашим образом жизни. " +
                    "Один из важных показателей при оценке избыточного веса — соотношение окружности талии к окружности бедер. " +
                    "Особенно опасен жир, наросший на животе и внутри. Абдоминальный жир скапливается вокруг внутренних органов, " +
                    "например, печени, мешая их работе и способствуя нарушению обмена веществ. Также чем больше жира на животе, " +
                    "тем больше вероятность развития сердечно-сосудистых проблем, так как жировая ткань в области живота участвует в " +
                    "обмене веществ организма и может способствовать образованию отложений в кровеносных сосудах. " +
                    "При выкуривании одной сигареты сердце человека начинает работать в ускоренном ритме, который " +
                    "сохраняется на протяжении 15 минут. Хватает всего нескольких затяжек, чтобы артериальное давление" +
                    "увеличилось на 5%, пульс участился на 14%. Влияние курения на сердечно-сосудистую систему проявляется " +
                    "также в том, что повышается свёртываемость и вязкость крови, изменения адгезивных свойств тромбоцитов, " +
                    "а это способствует усилению тромбообразования. " +
                    "Попавший в кровь этанол на протяжении 5-7 часов заставляет сердце работать с повышенной нагрузкой. " +
                    "Пагубное влияние алкоголя на сердечно-сосудистую систему связано с учащением сердцебиения, " +
                    "повышением кровяного давления и нарушением кровообращения. На сосуды алкоголь действует двухфазно: " +
                    "сначала сильно расширяет, а потом сужает. Это приводит к чрезмерным нагрузкам на сердце и нарушению его работы.";
            }
            if (prediction == 6)
            {
                result = "у вас есть риск развития сердечно-сосудистых заболеваний, связанный c повышенным коэффицентом атерогенности и " +
                    "вашим образом жизни. " +
                    "Коэффициент атерогенности – отношение \"плохого\" холестерола к \"хорошему\", характеризующее риск развития " +
                    "сердечно-сосудистых заболеваний. Чем больше значение \"плохого\" холестерола и меньше знаечние " +
                    "\"хорошего\" холестерола, тем больше риск развития ССЗ. " +
                    "При выкуривании одной сигареты сердце человека начинает работать в ускоренном ритме, который " +
                    "сохраняется на протяжении 15 минут. Хватает всего нескольких затяжек, чтобы артериальное давление" +
                    "увеличилось на 5%, пульс участился на 14%. Влияние курения на сердечно-сосудистую систему проявляется " +
                    "также в том, что повышается свёртываемость и вязкость крови, изменения адгезивных свойств тромбоцитов, " +
                    "а это способствует усилению тромбообразования. " +
                    "Попавший в кровь этанол на протяжении 5-7 часов заставляет сердце работать с повышенной нагрузкой. " +
                    "Пагубное влияние алкоголя на сердечно-сосудистую систему связано с учащением сердцебиения, " +
                    "повышением кровяного давления и нарушением кровообращения. На сосуды алкоголь действует двухфазно: " +
                    "сначала сильно расширяет, а потом сужает. Это приводит к чрезмерным нагрузкам на сердце и нарушению его работы.";
            }
            if (prediction == 7)
            {
                result = "у вас есть риск развития сердечно-сосудистых заболеваний, связанный с повышенным индексом талии/бедра, " +
                    "повышенным коэффицентом атерогенности и вашим образом жизни. " +
                    "Один из важных показателей при оценке избыточного веса — соотношение окружности талии к окружности бедер. " +
                    "Особенно опасен, так называемый абдоминальный жир - отложившийся на животе и внутренних органах, " +
                    "например, печени, мешая их работе и способствуя нарушению обмена веществ. Также чем больше жира на животе, " +
                    "тем больше вероятность развития сердечно-сосудистых проблем, так как жировая ткань в области живота участвует в " +
                    "обмене веществ организма и может способствовать образованию отложений в кровеносных сосудах. " +
                    "Коэффициент атерогенности – отношение \"плохого\" холестерола к \"хорошему\", характеризующее риск развития " +
                    "сердечно-сосудистых заболеваний. Чем больше значение \"плохого\" холестерола и меньше знаечние " +
                    "\"хорошего\" холестерола, тем выше риск формирования атерогенных бляшек на поверхности сосудов, что наружает ток крови " +
                    "и в сочетании с повышенным тромбообразованием может привести к закупориванию сосудов тромбами." +
                    "При выкуривании одной сигареты сердце человека начинает работать в ускоренном ритме, который " +
                    "сохраняется на протяжении 15 минут. Хватает всего нескольких затяжек, чтобы артериальное давление" +
                    "увеличилось на 5%, пульс участился на 14%. Влияние курения на сердечно-сосудистую систему проявляется " +
                    "также в том, что повышается свёртываемость и вязкость крови, изменения адгезивных свойств тромбоцитов, " +
                    "а это способствует усилению тромбообразования. " +
                    "Попавший в кровь этанол на протяжении 5-7 часов заставляет сердце работать с повышенной нагрузкой. " +
                    "Пагубное влияние алкоголя на сердечно-сосудистую систему связано с учащением сердцебиения, " +
                    "повышением кровяного давления и нарушением кровообращения. На сосуды алкоголь действует двухфазно: " +
                    "сначала сильно расширяет, а потом сужает. Это приводит к чрезмерным нагрузкам на сердце и нарушению его работы. " +
                    "Все эти факторы в сочетании приводят к возникновению ишемической болезни сердца, инфаркта, инсульта.";
            }

            return result;
        }

        private string GetRecomendateByClass(float prediction)
        {
            var result = "";

            if (prediction == 1 || prediction == 4 || prediction == 5 || prediction == 7)
            {
                result += "Для того, чтобы понизить индекс талии/бедра, необходимо соблюдать диету и активно заниматься спортом. " +
                    "Стараться исключить из своего рациона или свести к минимому употребление мучного, сладкого, жирной и вредной пищи. " +
                    "Почаще заниматься активным отдыхом: прогулки, легкие пробежки, поездки на природу/в походы. Стараться не упортреблять " +
                    "тяжелую пищу в позднее время перед сном, так как это неизбежно уходит в лишний вес. ";
            }
            if (prediction == 2 || prediction == 4 || prediction == 6 || prediction == 7)
            {
                result += "Для того, чтобы понизить уровень  \"плохого\" холестерин и повысить уровень \"хорошего\", " +
                    "необходимо: уменьшить потребление насыщенных жиров (ограничить потребление красного мяса, сливочного масла, " +
                    "жирных молочных продуктов и полуфабрикатов); добавить в рацион пищевые волокна (овсянка, фрукты, овощи и бобовые); " +
                    "избегать транс жиров (выпечка, фастфуд); регулярно заниматься физической активностью, такие как ходьба, бег, плавание " +
                    "или езда на велосипеде; отказаться от курения; оргничить употребление алкоголя; управлять стрессом, практики релаксации, " +
                    "такие как йога, медитация или глубокое дыхание, могут помочь снизить уровень стресса и улучшить состояние сердечно-сосудистой " +
                    "системы. ";
            }
            if (prediction == 3 || prediction == 5 || prediction == 6 || prediction == 7)
            {
                result += "Даже при хороших показателях анализов и отличном самочувствии следует задуматсья, как сильно нездоровый образ " +
                    "жизни влияет на состояние здоровья человека. В среднем около 20% людей, ведущих нездоровый образ жизни, умирают от " +
                    "ишемической болезни сердца. Чтобы эффективнее бросить курить, используйте антистрессовые игрушки, карандаши или эспандеры; кушайте " +
                    "жвачки без сахара, морковные палочки, орехи, семечки. Чтобы эффективнее бросить употребление алкоголя, заменйяте его на " +
                    "вкусные и полезные напитки, которые можно приготовить дома (овощные соки, смузи, а также чай). ";
            }

            result += "Здоровье любого человека – это его счастье и богатство. Поддерживать его важно: не иметь вредных привычек, " +
                "вести активный образ жизни, правильно питаться, следить за собой и своим телом, периодически проверяться на наличие " +
                "болезней. Забота о своем здоровье – залог радости, успеха и гармонии.";

            return result;
        }

        private UpperAndLowerBounds GetHDLValue(int age, string gender)
        {
            var dictionary = new Dictionary<AgeAndGender, UpperAndLowerBounds>();

            dictionary.Add(new AgeAndGender(20, "male"), new UpperAndLowerBounds(0.78, 1.63));
            dictionary.Add(new AgeAndGender(20, "female"), new UpperAndLowerBounds(0.91, 1.91));
            dictionary.Add(new AgeAndGender(25, "male"), new UpperAndLowerBounds(0.78, 1.63));
            dictionary.Add(new AgeAndGender(25, "female"), new UpperAndLowerBounds(0.85, 2.04));
            dictionary.Add(new AgeAndGender(30, "male"), new UpperAndLowerBounds(0.80, 1.63));
            dictionary.Add(new AgeAndGender(30, "female"), new UpperAndLowerBounds(0.96, 2.15));
            dictionary.Add(new AgeAndGender(35, "male"), new UpperAndLowerBounds(0.72, 1.63));
            dictionary.Add(new AgeAndGender(35, "female"), new UpperAndLowerBounds(0.93, 1.99));
            dictionary.Add(new AgeAndGender(40, "male"), new UpperAndLowerBounds(0.75, 1.60));
            dictionary.Add(new AgeAndGender(40, "female"), new UpperAndLowerBounds(0.88, 2.12));
            dictionary.Add(new AgeAndGender(45, "male"), new UpperAndLowerBounds(0.70, 1.73));
            dictionary.Add(new AgeAndGender(45, "female"), new UpperAndLowerBounds(0.88, 2.28));
            dictionary.Add(new AgeAndGender(50, "male"), new UpperAndLowerBounds(0.78, 1.66));
            dictionary.Add(new AgeAndGender(50, "female"), new UpperAndLowerBounds(0.88, 2.25));
            dictionary.Add(new AgeAndGender(55, "male"), new UpperAndLowerBounds(0.72, 1.63));
            dictionary.Add(new AgeAndGender(55, "female"), new UpperAndLowerBounds(0.96, 2.38));
            dictionary.Add(new AgeAndGender(60, "male"), new UpperAndLowerBounds(0.72, 1.84));
            dictionary.Add(new AgeAndGender(60, "female"), new UpperAndLowerBounds(0.96, 2.35));
            dictionary.Add(new AgeAndGender(200, "male"), new UpperAndLowerBounds(0.78, 1.91));
            dictionary.Add(new AgeAndGender(200, "female"), new UpperAndLowerBounds(0.98, 2.38));

            foreach (var pair in dictionary)
            {
                if (age <= pair.Key.Age && pair.Key.Gender.Equals(gender))
                {
                    return pair.Value;
                }
            }

            return null;
        }

        private UpperAndLowerBounds GetLDLValue(int age, string gender)
        {
            var dictionary = new Dictionary<AgeAndGender, UpperAndLowerBounds>();

            dictionary.Add(new AgeAndGender(20, "male"), new UpperAndLowerBounds(1.67, 3.37));
            dictionary.Add(new AgeAndGender(20, "female"), new UpperAndLowerBounds(1.53, 3.55));
            dictionary.Add(new AgeAndGender(25, "male"), new UpperAndLowerBounds(1.71, 3.81));
            dictionary.Add(new AgeAndGender(25, "female"), new UpperAndLowerBounds(1.48, 4.12));
            dictionary.Add(new AgeAndGender(30, "male"), new UpperAndLowerBounds(1.81, 4.27));
            dictionary.Add(new AgeAndGender(30, "female"), new UpperAndLowerBounds(1.84, 4.25));
            dictionary.Add(new AgeAndGender(35, "male"), new UpperAndLowerBounds(2.02, 4.79));
            dictionary.Add(new AgeAndGender(35, "female"), new UpperAndLowerBounds(1.81, 4.04));
            dictionary.Add(new AgeAndGender(40, "male"), new UpperAndLowerBounds(2.10, 4.90));
            dictionary.Add(new AgeAndGender(40, "female"), new UpperAndLowerBounds(1.94, 4.45));
            dictionary.Add(new AgeAndGender(45, "male"), new UpperAndLowerBounds(1.25, 4.82));
            dictionary.Add(new AgeAndGender(45, "female"), new UpperAndLowerBounds(1.92, 4.51));
            dictionary.Add(new AgeAndGender(50, "male"), new UpperAndLowerBounds(2.51, 5.23));
            dictionary.Add(new AgeAndGender(50, "female"), new UpperAndLowerBounds(2.05, 4.82));
            dictionary.Add(new AgeAndGender(55, "male"), new UpperAndLowerBounds(2.31, 5.10));
            dictionary.Add(new AgeAndGender(55, "female"), new UpperAndLowerBounds(2.28, 5.21));
            dictionary.Add(new AgeAndGender(60, "male"), new UpperAndLowerBounds(2.28, 5.26));
            dictionary.Add(new AgeAndGender(60, "female"), new UpperAndLowerBounds(2.59, 5.80));
            dictionary.Add(new AgeAndGender(200, "male"), new UpperAndLowerBounds(2.54, 5.44));
            dictionary.Add(new AgeAndGender(200, "female"), new UpperAndLowerBounds(2.38, 5.72));

            foreach (var pair in dictionary)
            {
                if (age <= pair.Key.Age && pair.Key.Gender.Equals(gender))
                {
                    return pair.Value;
                }
            }

            return null;
        }

        private UpperAndLowerBounds GetCAValue(int age, string gender)
        {
            var dictionary = new Dictionary<AgeAndGender, UpperAndLowerBounds>();

            dictionary.Add(new AgeAndGender(30, "male"), new UpperAndLowerBounds(0.00, 2.20));
            dictionary.Add(new AgeAndGender(30, "female"), new UpperAndLowerBounds(0.00, 2.50));
            dictionary.Add(new AgeAndGender(40, "male"), new UpperAndLowerBounds(1.80, 4.40));
            dictionary.Add(new AgeAndGender(40, "female"), new UpperAndLowerBounds(2.00, 4.92));
            dictionary.Add(new AgeAndGender(60, "male"), new UpperAndLowerBounds(0.00, 3.20));
            dictionary.Add(new AgeAndGender(60, "female"), new UpperAndLowerBounds(0.00, 3.50));
            dictionary.Add(new AgeAndGender(200, "male"), new UpperAndLowerBounds(0.00, 3.30));
            dictionary.Add(new AgeAndGender(200, "female"), new UpperAndLowerBounds(0.00, 3.40));

            foreach (var pair in dictionary)
            {
                if (age <= pair.Key.Age && pair.Key.Gender.Equals(gender))
                {
                    return pair.Value;
                }
            }

            return null;
        }

        private UpperAndLowerBounds GetWHIValue(string gender)
        {
            if (gender.Equals("male"))
            {
                return new UpperAndLowerBounds(0.9, 1.00);
            }
            else
            {
                return new UpperAndLowerBounds(0.85, 0.90);
            }
        }
    }

    public class AgeAndGender
    {
        public AgeAndGender(int age, string gender)
        {
            Age = age;
            Gender = gender;
        }

        public int Age { get; set; }
        public string Gender { get; set; }
    }

    public class UpperAndLowerBounds
    {
        public UpperAndLowerBounds(double lower, double upper)
        {
            Upper = upper;
            Lower = lower;
        }

        public double Upper {  get; set; }
        public double Lower {  get; set; }
    }
}
