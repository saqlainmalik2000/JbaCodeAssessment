using JbaCodeAssessment.Models;
using JbaCodeAssessment.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JbaCodeAssessment.Services.Implementations
{
    /// <summary>
    /// This class specifically handles the file type given in the coding exercise, 
    /// which uses a specfic data structure and data delimeter.
    /// Other file types will have their own implementations.
    /// </summary>
    public class FileProcessor_PreFileType : IFileProcessor
    {
        private const char Delimiter = ' ';
        private const string GridReferencePrefix = "Grid-ref=";

        public async Task<ReportDto> ProcessAsync(StreamReader sr)
        {
            // get report header 
            var report = await Task.Run(() => GetReportHeader(sr));

            // reset the stream and the buffer to insure the position hasn't moved passed an expected point
            sr.DiscardBufferedData();
            sr.BaseStream.Seek(0, SeekOrigin.Begin);

            // get report data
            report.Data = await Task.Run(() => GetReportData(sr, report.StartYear, report.EndYear));

            return report;
        }


        private ReportDto GetReportHeader(StreamReader sr)
        {
            var report = new ReportDto();
            var line = sr.ReadLine();
            while (!line.StartsWith(GridReferencePrefix))
            {
                report.HeaderText += line + "\n";
                if (line.Contains("Years"))
                {
                    var years = GetReportYearRange(line);
                    report.StartYear = years.startYear;
                    report.EndYear = years.endYear;
                }
                line = sr.ReadLine();
            }
            report.HeaderText = report.HeaderText.Remove(report.HeaderText.Length - 1, 1);
            return report;
        }

        private List<ReportDataDto> GetReportData(StreamReader sr, int startYear, int endYear)
        {
            var reportData = new List<ReportDataDto>();

            var lines = new List<string>();
            var line = sr.ReadLine();

            while (line != null && !line.StartsWith(GridReferencePrefix))
                line = sr.ReadLine();

            while (line != null)
            {
                if (line.StartsWith(GridReferencePrefix))
                {
                    if (lines.Any())
                    {
                        reportData.AddRange(GetReportDataFromStreamInput(lines, startYear, endYear));
                        lines.Clear();
                    }
                }

                lines.Add(line);
                line = sr.ReadLine();
            }

            if (lines.Any())
            {
                reportData.AddRange(GetReportDataFromStreamInput(lines, startYear, endYear));
                lines.Clear();
            }

            return reportData;
        }

        private (int startYear, int endYear) GetReportYearRange(string yearLine)
        {
            var years = yearLine.Split("]");

            if (years.Length < 2)
                throw new Exception($"Missing years declaration in import file: {yearLine}");

            years = years[1].Replace("Years=", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Split('-');

            if (years.Length < 2)
                throw new Exception($"Missing years declaration in import file: {yearLine}");

            if (!int.TryParse(years[0], out int startYear))
                throw new Exception("Invalid start year value");

            if (!int.TryParse(years[1], out int endYear))
                throw new Exception("Invalid end year value");

            if (startYear > endYear)
                throw new Exception("End year cannot be earlier than the start year");

            return (startYear, endYear);
        }

        private List<ReportDataDto> GetReportDataFromStreamInput(List<string> dataLines, int startYear, int endYear)
        {
            var reportData = new List<ReportDataDto>();
            var currentYear = startYear;
            var gridReference = GetGridReference(dataLines.First());

            dataLines.RemoveAt(0);

            foreach (var dataLine in dataLines)
            {
                if (currentYear > endYear)
                    continue;

                if (!string.IsNullOrWhiteSpace(dataLine))
                {
                    var month = 1;
                    var monthlyValues = dataLine.Split(Delimiter);
                    if (!monthlyValues.Any())
                        continue;

                    foreach(var monthlyValue in monthlyValues)
                    {
                        if (!string.IsNullOrWhiteSpace(monthlyValue))
                        {
                            if (!int.TryParse(monthlyValue, out int value))
                                throw new Exception($"Incorrect format for percipitation value in line: {dataLine}");

                            reportData.Add(new ReportDataDto { Date = new DateTime(currentYear, month, 1).Date, XRef = gridReference.x, YRef = gridReference.y, Value = value });
                            month++;
                        }
                    }
                    currentYear++;
                }
            }

            return reportData;
        }

        private (int x, int y) GetGridReference(string referenceString)
        {
            if (!referenceString.StartsWith(GridReferencePrefix))
                throw new Exception($"Grid reference string is not in the correct format {referenceString}");

            var gridReference = referenceString.Replace(" ", string.Empty).Replace(GridReferencePrefix, string.Empty).Split(',');

            if (gridReference.Length < 2 || !int.TryParse(gridReference[0], out var x) || !int.TryParse(gridReference[1], out var y))
                throw new Exception($"Grid reference string is not in the correct format {referenceString}");

            return (x, y);
        }
    }
}
