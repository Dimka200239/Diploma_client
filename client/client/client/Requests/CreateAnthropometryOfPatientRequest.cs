namespace client.Requests
{
    public class CreateAnthropometryOfPatientRequest
    {
        public int PatientId { get; set; }
        public string Role { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public int Age { get; set; }
        public double Hip { get; set; }
        public double Waist { get; set; }
    }
}
