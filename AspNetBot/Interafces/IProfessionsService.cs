using AspNetBot.DTO;

namespace AspNetBot.Interafces
{
    public interface IProfessionsService
    {
        Task<IEnumerable<ProfessionDto>> GetAllAsync(bool tracking = true);
        Task<ProfessionDto> GetByIdAsync(int id,bool tracking = true);
        Task CreateAsync(string name);
        Task DeleteAsync(int id); 
    }
}
