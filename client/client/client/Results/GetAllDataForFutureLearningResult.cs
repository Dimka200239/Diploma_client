using client.Model;
using System.Collections.Generic;

namespace client.Results
{
    public class GetAllDataForFutureLearningResult : BaseResult
    {
        public List<DataForFutureLearning> DataForFutureLearnings { get; set; }
    }
}
