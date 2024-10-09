using AspNetBot.DTO;
using AspNetBot.Entities;
using AspNetBot.Models;

namespace AspNetBot.Interafces
{
    public interface IBotUserService
    {
        Task<IEnumerable<BotUserDto>> getAll(bool tracking = true);
        Task<IEnumerable<BotUserDto>> getAllByProfession(int professionId, bool tracking = true);
        Task<IEnumerable<BotUserDto>> getAllByProfession(string professionName,bool tracking = true);
        Task<BotUserDto> getById(string botUserId, bool tracking = true);
        Task<BotUserDto> getByChatId(long botUserChatId, bool tracking = true);
        Task delete(long botUserCahtId);
        Task delete(string botUserId);
        Task delete(BotUser botUser);
        Task set(BotUserCreationModel model);
        Task<BotUserDto> setUserProfession(long userCatId,int professionId);
       
    }
}
