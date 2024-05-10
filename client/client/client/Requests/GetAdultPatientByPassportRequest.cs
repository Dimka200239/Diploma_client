using System;

namespace client.Requests
{
    public class GetAdultPatientByPassportRequest
    {
        public string Series { get; set; }
        public string Number { get; set; }
        public DateTime DateOfIssue { get; set; }
    }
}
