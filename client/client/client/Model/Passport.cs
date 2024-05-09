using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace client.Model
{
    [Table("Passports")]
    public class Passport
    {
        [Key]
        public int AdultPatientId { get; set; }
        [Required(ErrorMessage = "Серия паспорта не может быть пустым полем")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Серия паспорта должна содержать 4 цифры")]
        public string Series { get; set; }
        [Required(ErrorMessage = "Номер паспорта не может быть пустым полем")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Номер паспорта должна содержать 6 цифр")]
        public string Number { get; set; }
        [Required(ErrorMessage = "Код подразделения паспорта не может быть пустым полем")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Код подразделения паспорта должна содержать 6 цифр")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Дата выдачи паспорта не может быть пустым полем")]
        public DateTime DateOfIssue { get; set; }
    }
}
