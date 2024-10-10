using AspNetBot.DTO;
using AspNetBot.Entities;
using AspNetBot.Exceptions;
using AspNetBot.Interafces;
using AspNetBot.Specifications;
using AutoMapper;
using System.Net;

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

        public async Task CreateAsync(string name)
        {
            await professions.AddAsync(new Profession { Name = name });
            await professions.SaveAsync();
        }

        public async Task DeleteAsync(int id)  
        {
            await professions.DeleteAsync(id);
            await professions.SaveAsync();
        }

        public async Task<IEnumerable<ProfessionDto>> GetAllAsync(bool tracking) => mapper.Map<IEnumerable<ProfessionDto>> (await professions.GetListBySpec(new ProfessionSpecs.GetAll(tracking)));

        public async Task<ProfessionDto> GetByIdAsync(int id, bool tracking = true)
        {
            var profession = await professions.GetByIDAsync(id) 
                ?? throw new HttpException("Invalid profession id",HttpStatusCode.BadRequest);
            return  mapper.Map<ProfessionDto>(profession);
        }
    }
}
