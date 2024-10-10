using AspNetBot.DTO;
using AspNetBot.Entities;
using AspNetBot.Mappers.Actions;
using AspNetBot.Models;
using AutoMapper;

namespace AspNetBot.Mappers
{
    public class BotUserProfile : Profile
    {
        public BotUserProfile()
        {
            CreateMap<BotUser, BotUserDto>()
               .ForMember(x => x.ProfessionName, opt => opt.MapFrom(x => x.Profession.Name));
            CreateMap<UserCreationModel, BotUser>()
               .AfterMap<BotUserProfileImageAction>();

        }
    }
}
