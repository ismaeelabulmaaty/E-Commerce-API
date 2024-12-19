using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services.Contract
{
    public interface IResponseCachService
    {

        //cach Data
        Task CachResponseAsync( string CachKey , object Response , TimeSpan ExpirTime);

        //Get Cached Data
        Task<string?> GetCachedResponse(string CachKey);
    }
}
