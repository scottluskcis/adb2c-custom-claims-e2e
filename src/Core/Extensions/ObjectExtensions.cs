using Newtonsoft.Json;

namespace Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object value, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(value, formatting);
        }

        public static T FromJson<T>(this string data)
        {
            return FromJson<T>(data, null);
        }

        public static T FromJson<T>(this string data, JsonSerializerSettings settings)
        {
            if (string.IsNullOrWhiteSpace(data))
                return default;

            return settings != null ? 
                JsonConvert.DeserializeObject<T>(data, settings) : 
                JsonConvert.DeserializeObject<T>(data);
        }
    }
}
