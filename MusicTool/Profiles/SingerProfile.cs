using AutoMapper;

namespace MusicTool.Profiles
{
    public class SingerProfile : Profile
    {
        public SingerProfile()
        {
            CreateMap<Models.Domain.Singer, Models.DTO.Singer>().ReverseMap();
        }
    }
}
