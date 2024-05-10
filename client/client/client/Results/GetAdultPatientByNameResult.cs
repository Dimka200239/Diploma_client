using client.Model;
using System.Collections.Generic;
using System.Windows.Documents;

namespace client.Results
{
    public class GetAdultPatientByNameResult : BaseResult
    {
        public List<GetPatientWithAddressItemList> AdultPatients { get; set; }
    }

    public class GetPatientWithAddressItemList
    {
        public AdultPatient AdultPatient { get; set; }
        public Address Address { get; set; }
    }
}
