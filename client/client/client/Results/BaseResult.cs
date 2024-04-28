using System.Collections.Generic;

namespace client.Results
{
    public abstract class BaseResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
