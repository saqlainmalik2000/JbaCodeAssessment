using AutoMapper;
using JbaCodeAssessment.Data;
using JbaCodeAssessment.Data.Entities;
using JbaCodeAssessment.Models;
using JbaCodeAssessment.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace JbaCodeAssessment.Services.Implementations
{
    public class FileManager : IFileManager, IDisposable
    {
        private readonly JbaContext _context;
        private readonly IMapper _mapper;

        public FileManager(JbaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Dispose()
        {
        }

        public async Task<ReportDto> ProcessAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File does not exist: {filePath}");

            StreamReader sr = new StreamReader(filePath);
            return await ProcessAsync(sr, HelperFunctions.GetFileType(filePath));
        }

        public async Task<ReportDto> ProcessAsync(StreamReader sr, FileTypes fileType)
        {
            var report = new ReportDto();

            switch (fileType)
            {
                case FileTypes.Pre:
                    var processor = new FileProcessor_PreFileType();
                    report = await processor.ProcessAsync(sr);
                    break;

                case FileTypes.NotSupported:
                    throw new Exception("File type is not supported");

                default:
                    throw new Exception("File type not recognized");
            }

            await SaveReport(report);
            return report;
        }

        /// <summary>
        /// For the purpose of this code assessment only percipitation grid data will be saved.
        /// No metadata relating to the file which was used for importing will be recorded. 
        /// In production systems, you would create a [Report] table which would have a one to many 
        /// relationship with a [ReportData] table ... along with other auditing tables.
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        private async Task SaveReport(ReportDto report)
        {
            var reportData = _mapper.Map<List<ReportData>>(report.Data);

            _context.ReportData.AddRange(reportData);
            await _context.SaveChangesAsync();
        }
    }
}
