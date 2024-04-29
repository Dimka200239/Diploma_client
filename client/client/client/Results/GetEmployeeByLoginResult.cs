using client.Model;

namespace client.Results
{
    public class GetEmployeeByLoginResult : BaseResult
    {
        public Employee Employee { get; set; }
    }
}
