using System;
using System.Collections;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbcontext;
        private Hashtable _repository;

        public UnitOfWork(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
            _repository= new Hashtable();
        }
        public async Task<int> CompleteAsync()
       => await _dbcontext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        =>await _dbcontext.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_repository.ContainsKey(type))
            {
                var Repository = new GenericRepository<TEntity>(_dbcontext);
                _repository.Add(type, Repository);
            }
            return _repository[type] as IGenericRepository<TEntity>;
           
            
        }
    }
}
