using Finaktiva.Application.Contracts.ISpecifications;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Finaktiva.Application.Contracts.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeString = null,
            bool disableTracking = true);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        T GetFirstOrDefaultNoAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(string id);
        Task<T> AddAsync(T entity);
        T Add(T entity);
        Task<T[]> AddRangeAsync(T[] entities);
        Task<List<T>> AddRangeAsync(List<T> entities);
        Task<T> UpdateAsync(T entity);
        Task<T> AttachAsync(T entity);
        Task DeleteAsync(T entity);
        void AddEntity(T entity);
        void AddEntityRange(T[] entities);
        void UpdateEntity(T entity);
        void AttachEntity(T entity);
        void DeleteEntity(T entity);
        Task DeleteRangeAsync(IReadOnlyList<T> entities);
        Task<T> GetByIdWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec, bool asNoTracking = false);
        Task<T> GetFirstWithSpec(ISpecification<T> spec, bool asNoTracking = false);
        Task<int> CountAsync(ISpecification<T> spec);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<T> PatchAsync(T entity, JsonPatchDocument jsonPatchDocument);
        IRepository<T> Include(Expression<Func<T, object>> includeExpression);
        IRepository<T> Include(string includeString);
        DbSet<T> GetQuery();
        IQueryable<T> GetQuery(ISpecification<T> spec);
        IQueryable<T> GetAllWithSpecQuery(ISpecification<T> spec, bool asNoTracking = false);

    }
}
