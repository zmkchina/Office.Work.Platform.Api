using AutoMapper;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.AutoMapperProfiles
{
    public class MemberPrizePunishProfile : Profile
    {
        public MemberPrizePunishProfile()
        {
            CreateMap<MemberPrizePunishEntity, MemberPrizePunishDto>();
        }
    }
}
