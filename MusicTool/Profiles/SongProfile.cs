using AutoMapper;

namespace MusicTool.Profiles
{
    public class SongProfile : Profile
    {
        public SongProfile()
        {
            CreateMap<Models.Domain.Song, Models.DTO.Song>().ReverseMap();
            CreateMap<Models.Domain.Song, Models.DTO.AutoSearch>().ReverseMap();
        }
    }
}
