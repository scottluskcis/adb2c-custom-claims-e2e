using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
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

        public static string GetValueForKey(this HttpRequest req, string key)
        {
            var result = "";
            if (req.Headers.ContainsKey(key))
                result = req.Headers[key];
            else if (req.Query.ContainsKey(key))
                result = req.Query[key];
            else if (req.Form?.ContainsKey(key) ?? false)
                result = req.Form[key];

            return result;
        }
    }
}
