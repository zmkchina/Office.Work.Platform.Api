using AutoMapper;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.AutoMapperProfiles
{
    public class PlanProfile : Profile
    {
        public PlanProfile()
        {
            CreateMap<PlanEntity, PlanInfoDto>();
        }
    }
}
