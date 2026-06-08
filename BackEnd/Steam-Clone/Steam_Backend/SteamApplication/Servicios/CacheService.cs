using Microsoft.Extensions.Caching.Memory;
using SteamApplication.Interfaces.Servicie;

namespace SteamApplication.Servicios
{
    public class CacheService(IMemoryCache memoryCache) : ICacheService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Clave de caché. Ejemplo: tokens:<value></param>
        /// <param name="expiration"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public T Create<T>(string key, TimeSpan expiration, T value)
        {
            try
            {
                var create = memoryCache.GetOrCreate(key, (factory) =>
                {
                    factory.SlidingExpiration = expiration;
                    return value;
                });
                return create is null ? throw new Exception("No se pudo establecer la caché") : create;
            }
            catch
            {
                throw;
            }
        }



        public bool Delete(string key)
        {
            try
            {
                memoryCache.Remove(key);
                return true;
            }
            catch
            { return false; }

        }

        public T? Get<T>(string key)
        {
            try
            {
                return memoryCache.Get<T>(key);

            }
            catch
            { throw; }
        }
    }
}
