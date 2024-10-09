using AspNetBot.DTO;
using AspNetBot.Entities;
using AutoMapper;

namespace AspNetBot.Mappers
{
    public class ProfessionsProfile :Profile
    {
        public ProfessionsProfile() 
        {
            CreateMap<Profession, ProfessionDto>()
                .ReverseMap();
        }
    }
}
