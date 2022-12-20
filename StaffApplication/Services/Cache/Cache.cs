using Microsoft.Extensions.Caching.Memory;

namespace StaffApplication.Services.Cache
{
    public class Cache
    {
        public MemoryCache MemoryCache { get; set; } = new MemoryCache(
            new MemoryCacheOptions
            {
                SizeLimit = 2048
            });
    }
}
