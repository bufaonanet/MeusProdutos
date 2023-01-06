using DevIO.Business.Core.Data;
using DevIO.Business.Core.Models;
using DevIO.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevIO.Infra.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly MeuDbContext _db;
        protected readonly DbSet<TEntity> _dbSet;

        protected Repository(MeuDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();    
        }
        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task Adicionar(TEntity entity)
        {
             _dbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            await SaveChanges();
        }

        public virtual async Task Remover(Guid id)
        {
            _db.Entry(new TEntity { Id = id}).State = EntityState.Detached;
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await _db.SaveChangesAsync();  
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }

}
