using client.Model;

namespace client.Results
{
    public class GetAdultPatientByIdWithAnthropometryAndLifestyleResult : BaseResult
    {
        public AnthropometryOfPatient AnthropometryOfPatient { get; set; }
        public Lifestyle Lifestyle { get; set; }
    }
}
