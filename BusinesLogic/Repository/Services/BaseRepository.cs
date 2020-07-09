using BusinesLogic.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinesLogic.Repository.Services
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _dbContext;
        public BaseRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;
        public virtual async Task<bool> Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression == null) return GetAll();
            return _dbContext.Set<TEntity>().Where(expression); 
        }

        public IQueryable<TEntity> GetAll() => _dbContext.Set<TEntity>().AsQueryable();

        public async Task<TEntity> GetById(Guid id) => await _dbContext.Set<TEntity>().FindAsync(id);

        public async Task<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> expression = null)
            => await Filter(expression).ToListAsync();

        public async Task<bool> Remove(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
