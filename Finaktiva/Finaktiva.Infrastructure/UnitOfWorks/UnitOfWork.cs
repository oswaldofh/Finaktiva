using Finaktiva.Application.Contracts.IRepositories;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Infrastructure.Persistences;
using Finaktiva.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;

namespace Finaktiva.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repository = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repository);
            }

            return (IRepository<TEntity>)_repositories[type];
        }


        public IDbContextTransaction BeginTransaction()
            => _context.Database.BeginTransaction();
    }
}
