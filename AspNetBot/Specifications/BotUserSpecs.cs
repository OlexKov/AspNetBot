using Ardalis.Specification;
using AspNetBot.Entities;


namespace AspNetBot.Specifications
{
    public class BotUserSpecs
    {
        public class GetByChatId : Specification<BotUser>
        {
            public GetByChatId(long chatId, bool tracking = true)
            {
                Query.Where(x => x.ChatId == chatId)
                .Include(x => x.Profession)
                .AsTracking(tracking);
            } 
        }

        public class GetAll : Specification<BotUser>
        {
            public GetAll(bool tracking = true)
            {
                Query.Include(x => x.Profession).AsTracking(tracking);
            }
        }

        public class GetByProfession : Specification<BotUser>
        {
            private GetByProfession(bool tracking = true)
            {
                Query.Include(x => x.Profession).AsTracking(tracking);
            }
            public GetByProfession(int professionId,bool tracking = true):this(tracking)
            {
                Query.Where(x => x.ProfessionId == professionId);
            }
            public GetByProfession(string professionName, bool tracking = true) : this(tracking)
            {
                Query.Where(x => x.Profession.Name == professionName);
            }
        }

        public class GetById : Specification<BotUser>
        {
            private GetById(bool tracking = true)
            {
                Query.Include(x => x.Profession).AsTracking(tracking);
            }
            public GetById(long id, bool tracking = true) : this(tracking)
            {
                Query.Where(x => x.ChatId == id);
            }
            public GetById(int id, bool tracking = true) : this(tracking)
            {
                Query.Where(x => x.Id == id);
            }
        }
    }
}
