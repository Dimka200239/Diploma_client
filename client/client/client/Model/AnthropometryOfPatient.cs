using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace client.Model
{
    [Table("AnthropometryOfPatients")]
    public class AnthropometryOfPatient
    {
        [Key]
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Role { get; set; } //Роль пользователя
        public double Height { get; set; } //Рост
        public double Weight { get; set; } //Вес
        public int Age { get; set; } //Возраст
        public double Waist { get; set; } //Объем талии
        public double Hip { get; set; } //Объем бедра
        public DateTime DateOfChange { get; set; }

        [ForeignKey("PatientId")]
        public AdultPatient AdultPatient { get; set; }
    }
}
