using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace client.Model
{
    [Table("BloodAnalysises")]
    public class BloodAnalysis
    {
        [Key]
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Role { get; set; } //Роль пользователя
        public double AmountOfCholesterol { get; set; } //Кол-во холестерина в крови
        public double HDL { get; set; } //Кол-во ЛПВП в крови
        public double LDL { get; set; } //Кол-во ЛПНП в крови
        public double VLDL { get; set; } //Кол-во ЛПОНП в крови
        public double AtherogenicityCoefficient { get; set; } //Коэффицент атерогенности
        public double BMI { get; set; } //Индекс массы тела
        public double WHI { get; set; } //Индекс талии/бедра
        public DateTime DateOfChange { get; set; }
        public int EmployeeId { set; get; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [ForeignKey("PatientId")]
        public AdultPatient AdultPatient { get; set; }
    }
}
