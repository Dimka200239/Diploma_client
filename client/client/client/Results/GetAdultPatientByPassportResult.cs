using System.Collections.Generic;

namespace client.Results
{
    public class GetAdultPatientByPassportResult : BaseResult
    {
        public List<GetPatientWithAddressItemList> AdultPatients { get; set; }
    }
}
