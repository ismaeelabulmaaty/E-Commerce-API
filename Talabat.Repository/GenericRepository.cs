using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {

        private readonly StoreContext _dbcontext;

        public GenericRepository( StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<T?> GetAsync(int id)
        {
            if (typeof(T) == typeof(Product))
            {
                return await _dbcontext.Set<Product>().Where(p => p.Id == id).Include(p => p.Brand).Include(c => c.Category).FirstOrDefaultAsync() as T;
            }

            return await _dbcontext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IReadOnlyList<T>)await _dbcontext.Set<Product>().Include(p => p.Brand).Include(c => c.Category).ToListAsync();
            }

            return await _dbcontext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetEntityWithSpecAsync(Ispecification<T> spec)
        {
            return await ApplySpecififcations(spec).FirstOrDefaultAsync();
            //SpecififcationEvalutor<T>.GerQuery(_dbcontext.Set<T>(),spec).FirstOrDefaultAsync();  
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(Ispecification<T> sepc)
        {

            return await ApplySpecififcations(sepc).ToListAsync();
           // SpecififcationEvalutor<T>.GerQuery(_dbcontext.Set<T>(), spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(Ispecification<T> spec)
        {
            return await ApplySpecififcations(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecififcations( Ispecification<T> spec)
        {
            return  SpecififcationEvalutor<T>.GerQuery(_dbcontext.Set<T>(), spec);
        }

        public async Task AddAsync(T item)
           =>  await _dbcontext.Set<T>().AddAsync(item);

       

        public void delete(T item)
          =>_dbcontext.Set<T>().Remove(item);

        public void Update(T item)
        =>_dbcontext.Set<T>().Update(item);
    }
}
