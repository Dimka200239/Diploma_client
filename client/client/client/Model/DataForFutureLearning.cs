using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace client.Model
{
    [Table("DataForFutureLearning")]
    public class DataForFutureLearning
    {
        [Key]
        public int Id { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public bool SmokeCigarettes { get; set; }
        public bool DrinkAlcohol { get; set; }
        public bool Sport { get; set; }
        public double AmountOfCholesterol { get; set; }
        public double HDL { get; set; }
        public double LDL { get; set; }
        public double AtherogenicityCoefficient { get; set; }
        public double WHI { get; set; }

        public int HasCVD { get; set; } //Класс опасности развития ССЗ
    }
}
