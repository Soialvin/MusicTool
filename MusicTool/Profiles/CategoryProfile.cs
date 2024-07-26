using AutoMapper;

namespace MusicTool.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Models.Domain.Category, Models.DTO.Category>().ReverseMap();
            CreateMap<Models.Domain.Category, Models.DTO.NavHomeCategory>().ReverseMap();
        }
    }
}
