using AspNetBot.DTO;
using AspNetBot.Entities;
using AspNetBot.Interafces;
using AspNetBot.Specifications;
using AutoMapper;

namespace AspNetBot.Services
{
    public class ProfessionsService : IProfessionsService
    {
        private readonly IRepository<Profession> professions;
        private readonly IMapper mapper;

        public ProfessionsService(IRepository<Profession> professions,IMapper mapper)
        {
            this.professions = professions;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<ProfessionDto>> GetAll(bool tracking) => mapper.Map<IEnumerable<ProfessionDto>> (await professions.GetListBySpec(new ProfessionSpecs.GetAll(tracking)));
        
    }
}
