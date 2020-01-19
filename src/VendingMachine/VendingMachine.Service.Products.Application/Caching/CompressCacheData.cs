using System.IO;
using System.Text.Json;

namespace VendingMachine.Service.Products.Application.Caching
{
    public static class CompressCacheData
    {
        private static JsonSerializerOptions _serializer = new JsonSerializerOptions
        {
            WriteIndented = false,
            IgnoreNullValues = true,
        };

        public static byte[] SerializeCache<T>(this T obj)
        {
            if (obj == null)
            {
                return null;
            }

            return JsonSerializer.SerializeToUtf8Bytes(obj, typeof(T), _serializer);
        }

        public static async System.Threading.Tasks.Task<T> DeserializeCacheAsync<T>(this byte[] arrBytes)
        {
            if (arrBytes == null)
                return await System.Threading.Tasks.Task.FromResult<T>(default);

            using var m = new MemoryStream(arrBytes);
            return await JsonSerializer.DeserializeAsync<T>(m, _serializer);
        }
    }
}
