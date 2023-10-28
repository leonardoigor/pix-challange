using SharedLibrary.Domain.Entities.Base;
using System.Data.Entity;

namespace Api.Repository.Base
{
    public class BaseRepository<TEntity, Tid>
        where Tid : struct
        where TEntity : BaseEntity
    {
        private DbContext _dbContext;
        public BaseRepository(DbContext ctx)
        {
            _dbContext = ctx;
            _dbContext.Database.Initialize(true);
            
        }
        private DbSet<TEntity> Model
        {
            get
            {
                return _dbContext.Set<TEntity>();
            }
        }
        public IQueryable<TEntity> Get()
        {
            return Model.AsNoTracking().AsQueryable();
        }
        public TEntity Add(TEntity entity)
        {
            return Model.Add(entity);
        }
        public TEntity Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            return entity;
        }
        public TEntity Delete(TEntity entity)
        {
            return Model.Remove(entity);
        }
    }
}
