using AutoMapper;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.AutoMapperProfiles
{
    public class SettingServerProfile : Profile
    {
        public SettingServerProfile()
        {
            CreateMap<SettingServerEntity, SettingServerDto>();
        }
    }
}
