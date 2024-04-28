using client.Model;
using System.Collections.Generic;

namespace client.Results
{
    public class GetAllAddressesResult : BaseResult
    {
        public List<Address> Addresses { get; set; }
    }
}
