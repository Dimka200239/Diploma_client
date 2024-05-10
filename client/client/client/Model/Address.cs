using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Xml.Linq;

namespace client.Model
{
    [Table("Addresses")]
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Role { get; set; }
        [Required(ErrorMessage = "Название города не может быть пустым полем")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Название города должно содержать минимум 3 символа")]
        public string City { get; set; }
        [Required(ErrorMessage = "Название улицы не может быть пустым полем")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Название улицы должно содержать минимум 3 символа")]
        public string Street { get; set; }
        public int House { get; set; }
        public int? Apartment { get; set; }
        public DateTime DateOfChange { get; set; }

        public string GetFullAddress => City + ", ул. " + Street + ", д. " + House;
    }
}
