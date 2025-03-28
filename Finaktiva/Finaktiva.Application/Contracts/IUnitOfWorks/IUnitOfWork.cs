using Finaktiva.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Finaktiva.Application.Contracts.IUnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> Complete();
        IDbContextTransaction BeginTransaction();
    }
}
