using System.Collections.Generic;
using System.Threading.Tasks;

namespace EfuWebApi.Interfaces
{
    public interface IEfuExtractorService
    {
        Task<List<string>> ExtractFileNamesAsync();
    }
}
