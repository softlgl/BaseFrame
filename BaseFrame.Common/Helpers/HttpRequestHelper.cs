using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BaseFrame.Common.Extension;

namespace BaseFrame.Common.Helpers
{
    public class HttpRequestHelper
    {
        /// <summary>
        /// post数据到指定url
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="header">请求头</param>
        /// <param name="data">请求数据</param>
        /// <returns>请求结果</returns>
        public static async Task<string> PostDataToUrl(string url, IDictionary<string, string> header, string data)
        {
            using (var hc = new HttpClient())
            {
                if (header != null && header.Count > 0)
                {
                    hc.DefaultRequestHeaders.Clear();
                    foreach (var ss in header)
                    {
                        hc.DefaultRequestHeaders.Add(ss.Key, ss.Value);
                    }
                }
                var res = hc.PostAsync(url, new StringContent(data,
                                        Encoding.UTF8,
                                        "application/x-www-form-urlencoded")).Result;


                var result = await res.Content.ReadAsStringAsync();
                return result;
            }
        }

        /// <summary>
        /// post数据到指定url
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="data">请求数据</param>
        /// <returns>请求结果</returns>
        public static async Task<string> PostDataToUrl(string url, string data)
        {
            return await PostDataToUrl(url, null, data);
        }

        /// <summary>
        /// post数据到指定url
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="header">请求头</param>
        /// <param name="data">请求数据</param>
        /// <returns>请求结果</returns>
        public static string PostDataToUrlSync(string url, string data)
        {
            var result = string.Empty;
            using (var hc = new HttpClient(new RetryHandler(new HttpClientHandler())))
            {
                hc.Timeout = TimeSpan.FromSeconds(30);
                var res = hc.PostAsync(url, new StringContent(data,
                                        Encoding.UTF8,
                                        "application/x-www-form-urlencoded")).Result;

                if (res.IsSuccessStatusCode)
                {
                   return  res.Content.ReadAsStringAsync().Result; 
                }
                else
                {
                    return $"请求失败,状态码{res.StatusCode}";
                }
            }
            return result;
        }

        /// <summary>
        /// post数据到指定url
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="args">请求数据</param>
        /// <returns>请求结果</returns>
        public static async Task<string> PostDataToUrl(string url, IDictionary<string, string> args)
        {
            return await PostDataToUrl(url, null, args);
        }

        /// <summary>
        /// post数据到指定url
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="header">请求头字典</param>
        /// <param name="contentType">请求内容类型</param>
        /// <param name="data">请求数据</param>
        /// <returns>请求结果</returns>
        public static string PostDataToUrl(string url, IDictionary<string, string> header, string contentType, string data)
        {
            using (var hc = new HttpClient())
            {
                if (header != null)
                {
                    foreach (var ss in header)
                    {
                        hc.DefaultRequestHeaders.Add(ss.Key, ss.Value);
                    }
                }
                var res = hc.PostAsync(url, new StringContent(data, Encoding.UTF8, contentType))?.Result;
                return res?.Content.ReadAsStringAsync()?.Result;
            }
        }

        /// <summary>
        /// post数据到指定url
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="header">请求头字典</param>
        /// <param name="args">请求参数字典</param>
        /// <returns>请求结果</returns>
        public static async Task<string> PostDataToUrl(string url, IDictionary<string, string> header, IDictionary<string, string> args)
        {
            using (var hc = new HttpClient())
            {
                if (header != null)
                {
                    foreach (var ss in header)
                    {
                        hc.DefaultRequestHeaders.Add(ss.Key, ss.Value);
                    }
                }
                var res = await hc.PostAsync(url, new FormUrlEncodedContent(args));
                return res?.Content.ReadAsStringAsync().Result;
            }
        }



        /// <summary>
        /// 从指定urlGet数据
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="data">请求数据</param>
        /// <returns></returns>
        public static async Task<string> GetFromUrl(string url, string data)
        {
            var requestUrl = url;
            if (!data.IsNullOrEmpty())
                requestUrl = "{0}?{1}".FormatString(url, data);

            using (var hc = new HttpClient())
            {
                return hc.GetStringAsync(requestUrl)?.Result;
            }
        }

        public static async Task<string> GetFromUrl(string url, IDictionary<string, string> header, string data)
        {
            var requestUrl = url;
            if (!data.IsNullOrEmpty())
                requestUrl = "{0}?{1}".FormatString(url, data);

            using (var hc = new HttpClient())
            {
                if (header != null)
                {
                    foreach (var ss in header)
                    {
                        hc.DefaultRequestHeaders.Add(ss.Key, ss.Value);
                    }
                }
                var res = hc.GetStringAsync(requestUrl)?.Result;
                return res;
            }
        }

    }

    public class RetryHandler : DelegatingHandler
    {
        // Strongly consider limiting the number of retries - "retry forever" is
        // probably not the most user friendly way you could respond to "the
        // network cable got pulled out."
        private const int MaxRetries = 3;

        public RetryHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        { }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            for (int i = 0; i < MaxRetries; i++)
            {
                response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
            }

            return response;
        }
    }
}
