using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Filters.CrimeFilters
{
    public class SelectCrimesToUserFilter : IRequestFilter<Crime>
    {
        public IQueryable<Crime> Apply(IQueryable<Crime> query, CrimeFilterRequest filterRequest)
        {
            if (filterRequest.MyMarks is null && filterRequest.EditMarks is null) return query;

            if(filterRequest.MyMarks is Guid && filterRequest.EditMarks is Guid)
            {
                query = query.Where(c => c.CreatorId == filterRequest.MyMarks || c.EditorId == filterRequest.EditMarks);
            }
            else if (filterRequest.MyMarks is Guid)
                query = query.Where(c => c.CreatorId == filterRequest.MyMarks);

            else if (filterRequest.EditMarks is Guid)
                query = query.Where(c => c.EditorId == filterRequest.EditMarks);

            return query;
        }
    }
}
