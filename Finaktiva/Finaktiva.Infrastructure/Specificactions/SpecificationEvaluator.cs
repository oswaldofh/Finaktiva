using Finaktiva.Application.Contracts.ISpecifications;
using Microsoft.EntityFrameworkCore;

namespace Finaktiva.Infrastructure.Specificactions
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            if (spec.Criteria != null)
                inputQuery = inputQuery.Where(spec.Criteria);

            inputQuery = spec.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));
            inputQuery = spec.IncludeStrings.Aggregate(inputQuery, (current, include) => current.Include(include));

            if (spec.OrderBy != null)
                inputQuery = inputQuery.OrderBy(spec.OrderBy);

            if (spec.OrderByDescending != null)
                inputQuery = inputQuery.OrderByDescending(spec.OrderByDescending);

            if (spec.IsPagingEnabled)
            {
                int totalRegs = inputQuery.Count();
                inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);
            }


            return inputQuery;
        }
    }
}
