using AutoMapper;
using Jba.UI.ViewModels;
using JbaCodeAssessment.Data.Entities;
using JbaCodeAssessment.Models;

namespace Jba.UI
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<ReportDataDto, PercipitationDataViewModel>().ReverseMap();
            CreateMap<ReportData, PercipitationDataViewModel>().ReverseMap();

        }
    }
}
