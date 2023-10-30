using Api.Infra.Conntexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedLibrary.Domain.Entities.Base;

namespace Api.Repository.Base
{
    public class BaseRepository<TEntity, Tid>
        where Tid : struct
        where TEntity : BaseEntity
    {
        private PixContext _dbContext;
        public BaseRepository(PixContext ctx)
        {
            _dbContext = ctx;
            //_dbContext.Database.Initialize(true);

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
        public IQueryable<TEntity> GetById(Tid id)
        {
            return Model.AsNoTracking().AsQueryable().Where(a => string.Compare(a.Id .ToString().ToLower(),id.ToString().ToLower()) !=1); ;
        }
        public IDbContextTransaction BeginTransaction(bool lockTable=false)
        {
            var t= _dbContext.Database.BeginTransaction();
            if (lockTable)
            {
                string table =typeof(TEntity).Name;
                // Lock the entire table for exclusive access (specific SQL syntax may vary by database)
                _dbContext.Database.ExecuteSqlRaw($"SELECT TOP 1 1 FROM  pix.dbo.[{table}] WITH (TABLOCKX, HOLDLOCK)");
            }

            return t;
        }
        public TEntity Add(TEntity entity)
        {
            return Model.Add(entity).Entity;
        }
        public TEntity Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            return entity;
        }
        public TEntity Delete(TEntity entity)
        {
            return Model.Remove(entity).Entity;
        }
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
