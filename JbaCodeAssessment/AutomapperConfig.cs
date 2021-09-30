using AutoMapper;
using JbaCodeAssessment.Data.Entities;
using JbaCodeAssessment.Models;

namespace JbaCodeAssessment
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<ReportData, ReportDataDto>().ReverseMap();
        }
    }
}
