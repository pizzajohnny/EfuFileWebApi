using System.Collections.Generic;
using System.Threading.Tasks;
using EfuWebApi.Models;

namespace EfuWebApi.Interfaces
{
    public interface IEfuItemSearchService
    {
        List<string> EfuEntries { get; }

        Task<ResponseMessage> FindMatchesAsync(string[] names);
        Task ParseListAsync();
    }
}