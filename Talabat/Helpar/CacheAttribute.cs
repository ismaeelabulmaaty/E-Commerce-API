using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services.Contract;

namespace Talabat.Helpar
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTimeInSeconds;
        

        public CacheAttribute( int ExpireTimeInSeconds )
        {
            _expireTimeInSeconds = ExpireTimeInSeconds;
            
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //Explicity
           var CacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCachService>();

            var CacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var CacheResponse = await CacheService.GetCachedResponse(CacheKey);

            if (!string.IsNullOrEmpty(CacheResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = CacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

           var ExcutedEndPointContext = await next.Invoke(); //Excute Endpoint

            if( ExcutedEndPointContext.Result is OkObjectResult result )
            {
               await CacheService.CachResponseAsync(CacheKey , result.Value ,TimeSpan.FromSeconds(_expireTimeInSeconds));
            }
        }


        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            //api/Products
            var KeyBuilder = new StringBuilder();

            foreach ( var (key,Value) in request.Query.OrderBy(O=>O.Key))
            {

                KeyBuilder.Append($"|{key}-{Value}");

            }
            return KeyBuilder.ToString();
        }
    }
}
