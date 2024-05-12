namespace client.Requests
{
    public class BloodAnalysisRequest
    {
        public int PatientId { get; set; }
        public string Role { get; set; }
        public double AmountOfCholesterol { get; set; }
        public double HDL { get; set; }
        public double LDL { get; set; }
        public double VLDL { get; set; }
        public double AtherogenicityCoefficient { get; set; }
        public double BMI { get; set; }
        public double WHI { get; set; }
    }
}
