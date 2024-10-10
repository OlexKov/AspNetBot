using AspNetBot.DTO;
using AspNetBot.Entities;
using AspNetBot.Models;

namespace AspNetBot.Interafces
{
    public interface IBotUserService
    {
        Task<IEnumerable<BotUserDto>> GetAllAsync(bool tracking = true);
        Task<IEnumerable<BotUserDto>> GetAllByProfessionAsync(int professionId, bool tracking = true);
        Task<IEnumerable<BotUserDto>> GetAllByProfessionAsync(string professionName,bool tracking = true);
        Task<BotUserDto> GetByIdAsync(int userId, bool tracking = true);
        Task<BotUserDto> GetByChatIdAsync(long chatId, bool tracking = true);
        Task DeleteAsync(long chatId);
        Task<BotUserDto> SetUserProfessionAsync(long chatId,int professionId);
       
    }
}
