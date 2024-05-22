using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace client.Model
{
    [Table("CorrelationValue")]
    public class CorrelationValue
    {
        [Key]
        public int Id { get; set; }
        public double SmokeCigarettes { get; set; }
        public double DrinkAlcohol { get; set; }
        public double Sport { get; set; }
        public double AmountOfCholesterol { get; set; }
        public double HDL { get; set; }
        public double LDL { get; set; }
        public double AtherogenicityCoefficient { get; set; }
        public double WHI { get; set; }
        public int CountOfData { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
