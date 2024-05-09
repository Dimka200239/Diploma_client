using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace client.Model
{
    [Table("AdultPatients")]
    public class AdultPatient
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя пациента не может быть пустым полем")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Имя пациента должно содержать минимум 3 символа")]
        public string Name { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Фамилия пациента не может быть пустым полем")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Фамилия пациента должно содержать минимум 3 символа")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Дата рождения пациента не может быть пустым полем")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Номер телефона пациента не может быть пустым полем")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Номер телефона должен содержать 11 символов")]
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }

        [Required]
        public string Role { get; set; } //Роль пользователя

        public string GetFullName => Name + " " + LastName + " " + MiddleName + " ";
    }
}
