using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace client.Model
{
    [Table("Lifestyles")]
    public class Lifestyle
    {
        [Key]
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Role { get; set; } //Роль пользователя

        public bool SmokeCigarettes { get; set; } //Курит ли сигареты
        public bool DrinkAlcohol { get; set; } //Пьет ли алкоголь
        public bool Sport { get; set; } //Занимается ли спортом
        public DateTime DateOfChange { get; set; }

        [ForeignKey("PatientId")]
        public AdultPatient AdultPatient { get; set; }
    }
}
