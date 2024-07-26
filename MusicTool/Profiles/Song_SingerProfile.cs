using AutoMapper;

namespace MusicTool.Profiles
{
    public class Song_SingerProfile : Profile
    {
        public Song_SingerProfile()
        {
            CreateMap<Models.Domain.Song_Singer, Models.DTO.Song_Singer>().ReverseMap();
        }
    }
}
