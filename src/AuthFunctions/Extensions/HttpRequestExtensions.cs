using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AuthFunctions.Extensions
{
    public static class HttpRequestExtensions
    {
        public static async Task<T> ReadContentAsync<T>(this HttpRequest req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var result = JsonConvert.DeserializeObject<T>(requestBody);
            return result;
        }

        public static void LogDebug(this ILogger log, HttpRequest req)
        {
            var headers = req.Headers
                .Select(s => $"key: {s.Key}, value: {s.Value}");

            var query = req.Query
                .Select(s => $"key: {s.Key}, value: {s.Value}");

            var cookies = req.Cookies
                .Keys;

            var requestInfo = new
            {
                Headers = headers,
                Query = query,
                Cookies = cookies,
                req.Method,
                Host = req.Host.Value,
                req.ContentType,
                req.ContentLength,
                req.Scheme
            };

            var json = JsonConvert.SerializeObject(requestInfo, Formatting.Indented);
            log.LogDebug($"Request Info: {json}");
        }
    }
}
