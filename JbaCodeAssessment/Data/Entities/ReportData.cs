using System;

namespace JbaCodeAssessment.Data.Entities
{
    public class ReportData : BaseEntity
    {
        public int XRef { get; set; }
        public int YRef { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }
    }
}
