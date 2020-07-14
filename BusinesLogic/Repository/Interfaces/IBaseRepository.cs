using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinesLogic.Repository.Interfaces
{
    public interface IBaseRepository<TEntity> : IQuerableRepository<TEntity> where TEntity : class 
    {
        Task<bool> Add(TEntity entity);
        Task<bool> Update(TEntity entity);
        Task<bool> Remove(TEntity entity);
        Task<bool> CommitAsync();
    }
}
