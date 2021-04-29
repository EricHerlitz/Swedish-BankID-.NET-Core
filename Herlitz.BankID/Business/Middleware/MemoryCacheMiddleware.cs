using Herlitz.BankID.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Herlitz.BankID.Business.Middleware
{
    public class MemoryCacheMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly CacheOptions _cacheOptions;

        public MemoryCacheMiddleware(
            RequestDelegate next,
            IMemoryCache memoryCache,
            IOptions<CacheOptions> options)
        {
            _next = next;
            _cache = memoryCache;
            _cacheOptions = options.Value;
        }

        // IMyScopedService is injected into Invoke
        public async Task Invoke(HttpContext httpContext)
        {
            // Before the request

            // The request
            await _next(httpContext);


            // After the request
            //if (!_cacheOptions.Enabled)
            //{
            //    // set the bearer token as key
            //    var cacheKey = httpContext.Request.Headers[Constants.OrderRef].ToString();

            //    _cache.Remove(cacheKey);
            //}
        }
    }
}
