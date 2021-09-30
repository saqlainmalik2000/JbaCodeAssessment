using JbaCodeAssessment.Models;
using System.IO;
using System.Threading.Tasks;

namespace JbaCodeAssessment.Services.Interfaces

{
    public interface IFileManager
    {
        /// <summary>
        /// Asynchronously process a file with the given path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<ReportDto> ProcessAsync(string filePath);

        /// <summary>
        /// Asynchronously process a file which has been read into a stream reader
        /// </summary>
        /// <param name="streamReader"></param>
        /// <returns></returns>
        Task<ReportDto> ProcessAsync(StreamReader sr, FileTypes fileType);
    }
}


