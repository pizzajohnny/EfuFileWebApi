using System.Collections.Generic;

namespace EfuWebApi.Models
{
    public class ResponseMessage
    {
        public List<string> DeletedFiles { get; set; }
        public List<string> FoundFiles { get; set; }
    }
}
