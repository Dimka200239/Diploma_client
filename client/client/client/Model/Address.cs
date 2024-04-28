using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace client.Model
{
    [Table("Addresses")]
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Role { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int House { get; set; }
        public int? Apartment { get; set; }
        public DateTime DateOfChange { get; set; }
    }
}
