using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace HBSIS.Padawan.Produtos.Infra.Repository.GenericRepository
{
    public abstract class GenericRepository<TEntity, TContext>
        : IGenericRepository<TEntity> where TEntity : BaseEntity
        where TContext : MainContext
    {
        private readonly TContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;
        
        public GenericRepository(TContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }
        
        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var update = _dbSet.Find(entity.Id);
            var entry = _dbContext.Entry(update);
            entry.CurrentValues.SetValues(entity);
            entry.State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        protected IQueryable<TEntity> Query() => _dbSet.AsNoTracking();
    }
}