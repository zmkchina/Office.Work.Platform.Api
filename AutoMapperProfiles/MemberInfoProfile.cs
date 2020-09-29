using AutoMapper;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.AutoMapperProfiles
{
    public class MemberInfoProfile : Profile
    {
        public MemberInfoProfile()
        {
            CreateMap<MemberInfoEntity,MemberInfoDto>();
        }
    }
}
