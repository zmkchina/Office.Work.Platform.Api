using AutoMapper;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.AutoMapperProfiles
{
    public class MemberAppraiseProfile : Profile
    {
        public MemberAppraiseProfile()
        {
            CreateMap<MemberAppraiseEntity, MemberAppraiseDto>();
        }
    }
}
