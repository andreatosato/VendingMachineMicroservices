using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace VendingMachine.Service.Machines.Application.Cachings
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

        public static object DeserializeCache<T>(this byte[] arrBytes)
        {
            if (arrBytes == null)
                return null;

            using var m = new MemoryStream(arrBytes);
            return JsonSerializer.DeserializeAsync<T>(m, _serializer);
        }
    }
}
