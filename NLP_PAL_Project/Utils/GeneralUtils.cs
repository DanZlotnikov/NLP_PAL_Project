using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NLP_PAL_Project.Utils
{
    public class GeneralUtils
    {
        public static int AccessKeyIndex = 0;

        public async static Task<dynamic> MakeRequest(HttpMethod method, string url, dynamic stringContent = null)
        {
            // circulate access keys for increased limit
            AccessKeyIndex = AccessKeyIndex >= Consts.CohereAccessKeys.Count - 2 ? 0 : AccessKeyIndex + 1;
            Console.WriteLine(AccessKeyIndex);
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Consts.CohereAccessKeys[AccessKeyIndex]);

            if (method == HttpMethod.Get)
            {
                var result = await client.GetAsync(url);
                return result;
            }
            else if (method == HttpMethod.Post)
            {
                dynamic ret = null;
                try
                {
                    var response = await client.PostAsync(
                        url,
                        stringContent
                    );
                    var jsonString = await response.Content.ReadAsStringAsync();
                    ret = JsonConvert.DeserializeObject<dynamic>(jsonString.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
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

        public static bool TryCleanAndConvertToDouble(string input, out double result)
        {
            // Define the pattern to match any non-numeric characters except for the period and space
            string pattern = @"[^0-9.]";

            // Use Regex.Replace to replace all special characters with an empty string
            string cleanedString = Regex.Replace(input, pattern, string.Empty);

            // Try to convert the cleaned string to a double
            return double.TryParse(cleanedString, out result);
        }
    }
}
