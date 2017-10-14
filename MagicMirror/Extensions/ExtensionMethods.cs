using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.Extensions
{
    public static class ExtensionMethods
    {
        public static async Task<TEntity> DeserializeResultAsync<TEntity>(this Task<HttpResponseMessage> response)
        {
            var result = await response;
            return await result.DeserializeResultAsync<TEntity>();
        }

        public static async Task<TEntity> DeserializeResultAsync<TEntity>(this HttpResponseMessage response)
        {

            response.EnsureSuccessStatusCode();

            var value = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TEntity>(value);
        }
    }
}
