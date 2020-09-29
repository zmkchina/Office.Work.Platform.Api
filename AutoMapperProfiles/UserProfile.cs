using AutoMapper;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, Lib.UserDto>();
        }
    }
}
