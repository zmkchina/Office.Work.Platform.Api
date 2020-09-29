using AutoMapper;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.AutoMapperProfiles
{
    public class NoteProfile:Profile
    {
        public NoteProfile()
        {
            CreateMap<NoteInfoEntity, NoteInfoDto>();
        }
    }
}
