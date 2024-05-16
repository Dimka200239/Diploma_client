using Microsoft.ML.Data;

namespace client.Model
{
    public class HealthData
    {
        [LoadColumn(0)] public float Gender;
        [LoadColumn(1)] public float Age;
        [LoadColumn(2)] public float SmokeCigarettes;
        [LoadColumn(3)] public float DrinkAlcohol;
        [LoadColumn(4)] public float Sport;
        [LoadColumn(5)] public float AmountOfCholesterol;
        [LoadColumn(6)] public float HDL;
        [LoadColumn(7)] public float LDL;
        [LoadColumn(8)] public float AtherogenicityCoefficient;
        [LoadColumn(9)] public float WHI;
        [LoadColumn(10)] public float HasCVD;
    }
}
