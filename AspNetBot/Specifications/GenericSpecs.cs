using Ardalis.Specification;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq.Expressions;
using AspNetBot.Entities;

namespace AspNetBot.Specifications
{
    public class GenericSpecs
    {
        public class GetPagination<T> : Specification<T> where T : class
        {
            public GetPagination(int skip, int take, bool tracking)
            {
                var specification = this as Specification<T>;
                switch (specification)
                {
                    case Specification<BotUser> spec:
                        spec.Query
                            .Include(x => x.Profession)
                            .Skip(skip)
                            .Take(take)
                            .AsTracking(tracking);
                        break;

                    default:
                        Query.Skip(skip)
                             .Take(take)
                             .AsTracking(tracking);
                        break;
                }

            }
        }
    }
}
