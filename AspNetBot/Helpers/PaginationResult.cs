using AspNetBot.Interafces;
using AspNetBot.Specifications;
using AutoMapper;

namespace AspNetBot.Helpers
{
    public class PaginationResult<T,S> where T  : class  where S : class
    {
        public List<T> Elements { get; set; } = [];
        public int TotalCount { get; set; }
        public  PaginationResult(IRepository<S> repository,IMapper mapper, int pageIndex, int pageSize)
        {
            TotalCount = repository.CountAsync().Result;
            if (pageSize > 0)
            {
                int totalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
                if (pageIndex > totalPages)
                    pageIndex = totalPages;
            }
            else pageSize = TotalCount;
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            var items = repository.GetListBySpec(new GenericSpecs.GetPagination<S>((pageIndex-1) * pageSize, pageSize, false)).Result;
            var itemsDto = mapper.Map<IEnumerable<T>>(items);
            Elements.AddRange(itemsDto);
        }
    }
}
