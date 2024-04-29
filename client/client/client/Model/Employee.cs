using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace client.Model
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Login { set; get; }
        public string Password { set; get; }
        public DateTime RegistrationDate { set; get; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public bool IsConfirm { get; set; }
    }
}
