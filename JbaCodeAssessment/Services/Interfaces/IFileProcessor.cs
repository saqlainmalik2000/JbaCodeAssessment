using JbaCodeAssessment.Models;
using System.IO;
using System.Threading.Tasks;

namespace JbaCodeAssessment.Services.Interfaces
{
    public interface IFileProcessor
    {
        Task<ReportDto> ProcessAsync(StreamReader sr);
    }
}
