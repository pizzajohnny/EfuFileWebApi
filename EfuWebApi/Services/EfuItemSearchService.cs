using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EfuWebApi.Interfaces;
using EfuWebApi.Models;

namespace EfuWebApi
{
    public class EfuItemSearchService : IEfuItemSearchService
    {
        private readonly IEfuExtractorService _efuExtractor;

        public EfuItemSearchService(IEfuExtractorService efuExtractor)
        {
            _efuExtractor = efuExtractor;
        }

        public List<string> EfuEntries { get; private set; }

        public async Task ParseListAsync()
        {
            EfuEntries = await _efuExtractor.ExtractFileNamesAsync();
        }

        public Task<ResponseMessage> FindMatchesAsync(string[] names)
        {
            return Task.Run(async () =>
            {
                //Task[] tasks = new Task[] { CheckForExisitingFilesAsync(names), CheckForDeletedFilesAsync(names) };
                var resolvedTasks = await Task.WhenAll(CheckForExisitingFilesAsync(names), CheckForDeletedFilesAsync(names));

                var responseMessage = new ResponseMessage { DeletedFiles = resolvedTasks[1], FoundFiles = resolvedTasks[0] };
                return responseMessage;
            });
        }

        private Task<List<string>> CheckForExisitingFilesAsync(string[] names)
        {
            return Task.Run(() =>
            {
                var foundFiles = new List<string>();

                foreach (var existingTitle in EfuEntries)
                {
                    foreach (var requestedTitle in names)
                    {
                        var isIndexed = (existingTitle.IndexOf(requestedTitle, StringComparison.OrdinalIgnoreCase) >= 0)
                            || existingTitle == requestedTitle;
                        if (isIndexed)
                            foundFiles.Add(requestedTitle);
                    }
                }

                return foundFiles;
            });
        }

        private Task<List<string>> CheckForDeletedFilesAsync(string[] names)
        {
            return Task.Run(() =>
            {
                // TODO: implement logic
                return new List<string>();
            });
        }
    }
}
