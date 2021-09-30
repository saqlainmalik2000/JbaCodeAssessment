using System.Collections.Generic;

namespace JbaCodeAssessment.Models
{
    public class ReportDto
    {
        public ReportDto()
        {
            Data = new List<ReportDataDto>();
        }

        public string HeaderText { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public List<ReportDataDto> Data { get; set; }
    }
}

