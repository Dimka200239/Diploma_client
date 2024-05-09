namespace client.Requests
{
    public class CreateAddressRequest
    {
        public int PatientId { get; set; }
        public string Role { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int House { get; set; }
        public int? Apartment { get; set; }
    }
}
