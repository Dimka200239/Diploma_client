namespace client.Requests
{
    public class UpdateAdultPatientRequest
    {
        public int AdultPatientId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
