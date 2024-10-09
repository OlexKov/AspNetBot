using AspNetBot.DTO;

namespace AspNetBot.Interafces
{
    public interface IProfessionsService
    {
        Task<IEnumerable<ProfessionDto>> GetAll(bool tracking = true);
    }
}
