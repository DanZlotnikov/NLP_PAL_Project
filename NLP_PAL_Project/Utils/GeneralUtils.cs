using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NLP_PAL_Project.Utils
{
    public class GeneralUtils
    {
        public async static Task<dynamic> MakeRequest(HttpMethod method, string url, dynamic stringContent = null)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Consts.GptAccessKey);

            if (method == HttpMethod.Get)
            {
                var result = await client.GetAsync(url);
                return result;
            }
            else if (method == HttpMethod.Post)
            {
                var response = await client.PostAsync(
                    url,
                    stringContent
                );
                var jsonString = await response.Content.ReadAsStringAsync();
                dynamic ret = JsonConvert.DeserializeObject<dynamic>(jsonString.ToString());
                return ret;
            }
            else
            {
                return null;
            }
        }

        public async static Task<dynamic> GetRequest(string url)
        {
            return await MakeRequest(HttpMethod.Get, url);
        }

        public async static Task<dynamic> PostRequest(string url, StringContent stringContent)
        {
            return await MakeRequest(HttpMethod.Post, url, stringContent);
        }
    }
}
