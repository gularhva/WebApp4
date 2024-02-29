using WebApp4.Abstractions.IRepositories;
using WebApp4.Entities.Common;

namespace WebApp4.Abstractions.IUnitOfWorks
{
    public interface IUnitOfWork:IDisposable
    {
        public Task<int> SaveAsync();
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity:BaseEntity;
    }
}
