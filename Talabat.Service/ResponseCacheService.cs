using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    public class ResponseCacheService : IResponseCachService
    {
        private readonly IDatabase _database;

        public ResponseCacheService(IConnectionMultiplexer Redis)
        {
            _database = Redis.GetDatabase(); 
        }
        public async Task CachResponseAsync(string CachKey, object Response, TimeSpan ExpirTime)
        {
            if (Response is null) return;
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var SerializeResponse = JsonSerializer.Serialize(Response , options);

         await _database.StringSetAsync(CachKey, SerializeResponse, ExpirTime);
        }

        public async Task<string?> GetCachedResponse(string CachKey)
        {
           var CacheResponse = await _database.StringGetAsync(CachKey);
            if (CacheResponse.IsNullOrEmpty) return null;
            return CacheResponse;
        }
    }
}
