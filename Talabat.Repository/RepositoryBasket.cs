using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Talabat.Repository
{
    public class RepositoryBasket : IBasketRepository
    {



        private readonly IDatabase _database;

        public RepositoryBasket(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }




        public async Task<bool> DeletBasketAsync(string basketId)
        {
           return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket= await _database.StringGetAsync(basketId);

           return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var CreatOrUpdate =await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            if (CreatOrUpdate is false) return null;

            return await GetBasketAsync(basket.Id);
        }
    }
}
