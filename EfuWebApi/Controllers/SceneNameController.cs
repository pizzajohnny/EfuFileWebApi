using System.Collections.Generic;
using System.Threading.Tasks;
using EfuWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EfuWebApi.Controllers
{
    [Route("scenes")]
    [ApiController]
    public class SceneNameController : ControllerBase
    {
        private readonly IEfuItemSearchService _searchService;

        public SceneNameController(IEfuItemSearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost]
        public async Task<IEnumerable<string>> Post([FromBody] string[] requestedTitles)
        {
            var searchResult = await _searchService.FindMatchesAsync(requestedTitles);

            return searchResult.FoundFiles;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            var searchResult = _searchService.EfuEntries;

            return searchResult;
        }
    }
}
