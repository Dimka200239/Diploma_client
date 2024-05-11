namespace client.Requests
{
    public class LifestyleRequest
    {
        public int PatientId { get; set; }
        public string Role { get; set; }
        public bool SmokeCigarettes { get; set; }
        public bool DrinkAlcohol { get; set; }
        public bool Sport { get; set; }
    }
}
