using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace BMT.Server.Infrastructure
{
    public class HttpClientHelper
    {
        public static async Task<HttpActionContent> GetResponseJson(string url)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            //HttpContent content = new StringContent();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            var result = new HttpActionContent() { Success = true };
            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                result.Content = responseJson;
            }
            else
            {
                result.Success = false;
                result.Content = response.StatusCode.ToString();
            }
            return result;
        }

        public static async Task<HttpActionContent> PostResponseJson(string url, string requestJson)
        {
            HttpContent httpContent = new StringContent(requestJson);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
            var result = new HttpActionContent() { Success = true };
            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                result.Content = responseJson;
            }
            else
            {
                result.Success = false;
                result.Content = response.StatusCode.ToString();
            }
            return result;
        }

    }

    public class HttpActionContent
    {
        public bool Success { get; set; }

        public string Content { get; set; }
    }
}
