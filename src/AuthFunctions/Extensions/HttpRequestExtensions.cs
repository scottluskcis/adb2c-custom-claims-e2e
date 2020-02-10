using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    }
}
