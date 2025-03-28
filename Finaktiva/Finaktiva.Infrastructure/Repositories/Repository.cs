using Finaktiva.Application.Contracts.IRepositories;
using Finaktiva.Application.Contracts.ISpecifications;
using Finaktiva.Infrastructure.Persistences;
using Finaktiva.Infrastructure.Specificactions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Finaktiva.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private List<Expression<Func<T, object>>> _includeExpressions = new();
        private List<string> _includeStrings = new();
        public Repository(ApplicationDbContext context) => _context = context;
        public async Task<T> AddAsync(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public T Add(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<T[]> AddRangeAsync(T[] entities)
        {
            _context.Set<T>().AddRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task<List<T>> AddRangeAsync(List<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public void AddEntity(T entity) => _context.Set<T>().Add(entity);
        public void AddEntityRange(T[] entities) => _context.Set<T>().AddRange(entities);

        public async Task<int> CountAsync(ISpecification<T> spec) => await ApplySpecification(spec).CountAsync();
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().Where(predicate).CountAsync();

        public IQueryable<T> ApplySpecification(ISpecification<T> spec) => SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public void DeleteEntity(T entity) => _context.Set<T>().Remove(entity);

        public async Task DeleteRangeAsync(IReadOnlyList<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec, bool asNoTracking = false)
        {
            if (asNoTracking)
                return await ApplySpecification(spec).AsNoTracking().ToListAsync();
            return await ApplySpecification(spec).ToListAsync();
        }
        public async Task<T> GetFirstWithSpec(ISpecification<T> spec, bool asNoTracking = false)
        {
            if (asNoTracking)
                return await ApplySpecification(spec).AsNoTracking().FirstOrDefaultAsync();
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().Where(predicate).ToListAsync();

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeString = null,
            bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking) query = query.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            return await query.ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().FirstOrDefaultAsync(predicate);
        public T GetFirstOrDefaultNoAsync(Expression<Func<T, bool>> predicate) => _context.Set<T>().FirstOrDefault(predicate);

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking) query = query.AsNoTracking();
            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            return await query.ToListAsync();
        }


        public IQueryable<T> GetAllWithSpecQuery(ISpecification<T> spec, bool asNoTracking = false)
        => asNoTracking ? ApplySpecification(spec).AsNoTracking() : ApplySpecification(spec);


        public async Task<T> GetByIdAsync(string id) => await _context.Set<T>().FindAsync(id);
        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);


        public async Task<T> GetByIdWithSpec(ISpecification<T> spec) => await ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public void UpdateEntity(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<T> AttachAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public IRepository<T> Include(Expression<Func<T, object>> includeExpression)
        {
            _includeExpressions.Add(includeExpression);
            return this;
        }
        public IRepository<T> Include(string includeString)
        {
            _includeStrings.Add(includeString);
            return this;
        }

        public void AttachEntity(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<T> PatchAsync(T entity, JsonPatchDocument jsonPatchDocument)
        {
            jsonPatchDocument.ApplyTo(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public DbSet<T> GetQuery()
        => _context.Set<T>();

        public IQueryable<T> GetQuery(ISpecification<T> spec)
        => ApplySpecification(spec);

    }
}
