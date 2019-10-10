using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NhanDien.IOTLink.Helper
{
    /// <summary>
    /// Http client request
    /// </summary>
    public static class HttpClient
    {

        /// <summary>
        /// Create URL and request content from url
        /// </summary>
        /// <param name="searchUrl">Search URL</param>
        /// <param name="token">Authentication</param>
        /// <param name="method">Method: GET, POST</param>
        /// <param name="timeOut">Timeout</param>
        /// <returns>String content of url</returns>
        public static string ReadRequest(string searchUrl, string method = "GET", int timeOut = 5000)
        {
            var date = DateTime.Now;
            // Http web request content url
            var webRequest = WebRequest.Create(searchUrl) as HttpWebRequest;
            webRequest.Method = method;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            var result = "";
            webRequest.ContinueTimeout = timeOut;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader responseReader = new StreamReader(responseStream))
                        {
                            result = responseReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                result = GetContentString(e);
                Console.WriteLine(string.Format("Request url {0} error", searchUrl));
            }
            return result;
        }

        /// <summary>
        /// Create URL and request content from url
        /// </summary>
        /// <param name="searchUrl">Search URL</param>
        /// <param name="token">Authentication</param>
        /// <param name="method">Method: GET, POST</param>
        /// <param name="timeOut">Timeout</param>
        /// <returns>String content of url</returns>
        public static async Task<string> ReadRequestAsync(string searchUrl, string method = "GET", int timeOut = 5000)
        {
            var date = DateTime.Now;
            // Http web request content url
            var webRequest = WebRequest.Create(searchUrl) as HttpWebRequest;
            webRequest.Method = method;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            var result = "";
            webRequest.ContinueTimeout = timeOut;
            try
            {
                using (var response = await webRequest.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader responseReader = new StreamReader(responseStream))
                        {
                            result = responseReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                result = GetContentString(e);
                Console.WriteLine(string.Format("Request url {0} error", searchUrl));
            }
            return result;
        }

        /// <summary>
        /// Create URL and request content from url
        /// </summary>
        /// <param name="searchUrl">Url to search</param>
        /// <param name="method">Method is get, post, put or patch</param>
        /// <param name="timeOut">Timeout request</param>
        /// <param name="authen">Authen token</param>
        /// <returns></returns>
        public static async Task<string> ReadUrlDirectAsync(string searchUrl, string method = "GET", int timeOut = 5000)
        {
            var date = DateTime.Now;
            // Http web request content url
            var webRequest = WebRequest.Create(searchUrl) as HttpWebRequest;
            webRequest.Method = method;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            var result = "";
            webRequest.ContinueTimeout = timeOut;
            try
            {
                var task = await webRequest.GetResponseAsync().ConfigureAwait(false);
                result = task.ResponseUri.AbsoluteUri;
            }
            catch (WebException e)
            {
                result = e.Response.ResponseUri.AbsoluteUri;
                Console.WriteLine(string.Format("Request url {0} error", searchUrl));
            }
            return result;
        }

        /// <summary>
        /// read method post request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<string> ReadPostRequestAsync(string url, string data = null)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var response = await client.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json"));
                return await response.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// Read bytes request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<byte[]> ReadBytesRequestAsync(string url)
        {
            byte[] bytes = null;
            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            try
            {
                using (var response = await webRequest.GetResponseAsync() as HttpWebResponse)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            bytes = reader.ReadBytes((int)response.ContentLength);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                bytes = GetContentBytes(ex);
                Console.WriteLine(string.Format("Request url {0} error", url));
            }
            return bytes;
        }

        /// <summary>
        /// Read bytes request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static byte[] ReadBytes(string url)
        {
            using (var client = new WebClient())
            {
                return client.DownloadData(new Uri(url));
            }
        }

        /// <summary>
        /// Read stream
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Stream ReadStream(string url)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                /*
                client.GetStreamAsync(url)
                client.UseDefaultCredentials = true;
                client.Proxy.Credentials = CredentialCache.DefaultCredentials;
                client.Headers.Add("UserAgent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36");
                client.Headers.Add("Sec-Fetch-Mode", @"no-cors");
                client.Headers.Add("Referer", @"https://google.map4d.vn/");
                client.Get
                client.DownloadString(url);*/
                try
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd(@"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36");
                    return client.GetStreamAsync(url).Result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
        }

        /// <summary>
        /// Read stream async
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<Stream> ReadStreamAsync(string url)
        {
            using (var client = new WebClient())
            {
                return await client.OpenReadTaskAsync(new Uri(url));
            }
        }

        /// <summary>
        /// Read bytes request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<byte[]> ReadBytesAsync(string url)
        {
            using (var client = new WebClient())
            {
                return await client.DownloadDataTaskAsync(new Uri(url));
            }
        }
        /// <summary>
        /// Get content when error request
        /// </summary>
        /// <param name="webException"></param>
        /// <returns></returns>
        private static string GetContentString(WebException webException)
        {
            string rs = null;
            if (webException.Status == WebExceptionStatus.ProtocolError)
            {
                var response = (HttpWebResponse)webException.Response;
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader responseReader = new StreamReader(responseStream))
                    {
                        rs = responseReader.ReadToEnd();
                    }
                }
                Console.WriteLine(webException.Message, webException);
            }
            else
            {
                Console.WriteLine(webException.Message, webException);
            }
            return rs;
        }

        /// <summary>
        /// Get content when error request
        /// </summary>
        /// <param name="webException"></param>
        /// <returns></returns>
        private static byte[] GetContentBytes(WebException webException)
        {
            byte[] rs = null;
            if (webException.Status == WebExceptionStatus.ProtocolError)
            {
                var response = (HttpWebResponse)webException.Response;
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        rs = reader.ReadBytes((int)response.ContentLength);
                    }
                }
                Console.WriteLine(webException.Message, webException);
            }
            else
            {
                Console.WriteLine(webException.Message, webException);
            }
            return rs;
        }
    }
}
