using EfuWebApi.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EfuWebApi.Services
{
    public class EfuExtractorService : IEfuExtractorService
    {
        private static string[] _allowedFileExtensions =
        {
            ".mp4", ".wmv", ".avi", ".mov", ".mkv", ".mpg", ".mpeg"
        };

        private readonly string _efuFilePath;
        private readonly string _regExPattern;

        public EfuExtractorService(
            IConfiguration configuration)
        {
            _efuFilePath = configuration.GetValue<string>("EfuFilePath");
            _regExPattern = configuration.GetValue<string>("RegExPattern");
        }

        public Task<List<string>> ExtractFileNamesAsync()
        {
            return Task.Run(() =>
            {
                // SEE: https://stackoverflow.com/a/5283044

                var fileNames = new List<string>();

                using (var parser = new TextFieldParser(_efuFilePath))
                {
                    parser.SetDelimiters(new string[] { "," });
                    parser.HasFieldsEnclosedInQuotes = true;

                    // skip first line
                    parser.ReadLine();

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (fields.Length > 1)
                        {
                            if (long.TryParse(fields[1], out long size) && size > 0)
                            {
                                if (_allowedFileExtensions.Any(extension => extension == Path.GetExtension(fields[0])))
                                {
                                    var filename = Path.GetFileName(fields[0]);
                                    var regex = new Regex(_regExPattern);
                                    if (regex.IsMatch(filename))
                                    {
                                        var match = regex.Match(filename);
                                        var matchedName = match.Groups[0].Value;
                                        if (filename.Equals(matchedName, StringComparison.OrdinalIgnoreCase))
                                        {
                                            filename = Path.GetFileNameWithoutExtension(filename);
                                            filename = filename.Replace(".", " ");
                                            fileNames.Add(filename);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return fileNames;
                }
            });
        }
    }
}
