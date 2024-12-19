using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {

        Task<T?> GetAsync(int id);

        Task<IReadOnlyList<T>> GetAllAsync();


        Task<T?> GetEntityWithSpecAsync(Ispecification<T> spec);

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(Ispecification<T> sepc);

        Task<int> GetCountAsync(Ispecification<T> spec);

        Task AddAsync(T item);

        void delete( T item);

        void Update( T item);   
    }  
}
