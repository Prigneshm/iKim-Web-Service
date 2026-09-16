using IKimWebService.Infrastructure.IService;
using IKimWebService.Infrastructure.Utils;
using System.Web.Hosting;
using System.IO;
using System;

namespace IKimWebService.Service
{
    public class FileReaderService : IFileReaderService
    {
        public string ReadFileContent(string filePath)
        {
            if (filePath.IsNullOrEmpty())
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            string physicalPath = HostingEnvironment.MapPath(filePath);

            if (physicalPath == null || !File.Exists(physicalPath))
                throw new FileNotFoundException($"File not found at path: {filePath}");

            return File.ReadAllText(physicalPath);
        }
    }
}
