using AutoMapper;

namespace MusicTool.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Models.Domain.Account, Models.DTO.Account>().ReverseMap();
        }
    }
}
