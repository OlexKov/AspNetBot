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
                Query.Where(x => x.UserId == chatId)
                .Include(x => x.Profession);
                if (!tracking) Query.AsNoTracking();
            } 
        }

        public class GetAll : Specification<BotUser>
        {
            public GetAll(bool tracking = true)
            {
                Query.Include(x => x.Profession);
                if (!tracking) Query.AsNoTracking();
            }
        }

        public class GetByProfession : Specification<BotUser>
        {
            private GetByProfession(bool tracking = true)
            {
                Query.Include(x => x.Profession);
                if (!tracking) Query.AsNoTracking();
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
                Query.Include(x => x.Profession);
                if (!tracking) Query.AsNoTracking();
            }
            public GetById(long id, bool tracking = true) : this(tracking)
            {
                Query.Where(x => x.UserId == id);
            }
            public GetById(string id, bool tracking = true) : this(tracking)
            {
                Query.Where(x => x.Id == id);
            }
        }
    }
}
