using Ardalis.Specification;
using AspNetBot.Entities;


namespace AspNetBot.Specifications
{
    public class ProfessionSpecs
    {
        public class GetAll : Specification<Profession>
        {
            public GetAll(bool tracking = true)
            {
                Query.AsTracking(tracking);
            }
        }
    }
}
