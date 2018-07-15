using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SammakEnterprise.MagazineStore.Services
{
    public class BaseDataService
    {
        private const string ROOT_URI = "http://magazinestore.azurewebsites.net/api/";

        protected HttpClient Client = new HttpClient();
        private readonly Dictionary<string, object> _inMemoryCache = new Dictionary<string, object>();

        public BaseDataService()
        {
            Client.BaseAddress = new Uri(ROOT_URI);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public async Task<T> GenericGetAsync<T>(string path) where T: class
        {
            try
            {
                T result = null;
                if (_inMemoryCache.ContainsKey(path)) {
                    Console.WriteLine($"Data from cache for: {path}");
                    result = _inMemoryCache[path] as T;
                    return result;
                }

                var response = await Client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    var x = response.Content;
                    result = await x.ReadAsAsync<T>();
                    _inMemoryCache[path] = result;
                }
                else
                {
                    var errorMessage = $"Unsuccess Response for: {ROOT_URI}{path}";
                    errorMessage += $"\nstatus code: {response.StatusCode.ToString()}";
                    throw (new Exception (errorMessage));
                }
                return result;
            }
            catch (WebException webExcp)
            {
                var errorMessage = $"WebException has been caught  for: {ROOT_URI}{path}";
                errorMessage += $"\nwebExcp: {webExcp.ToString()}";

                var status = webExcp.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    errorMessage += $"\nThe server returned protocol error ";
                    var httpResponse = (HttpWebResponse)webExcp.Response;
                    errorMessage += $"\n{(int)httpResponse.StatusCode} - {httpResponse.StatusCode}";
                }
                throw (new Exception (errorMessage));
            }
            catch (Exception ex)
            {
                var errorMessage = $"An Exception has been caught for: {ROOT_URI}{path}";
                var ex2 = new Exception(errorMessage, ex);
                throw ex2;
            }
        }

        public async Task<TResult> GenericPostAsync<TPayload, TResult>(string path, TPayload payload) 
            where TPayload : class 
            where TResult : class
        {
            try
            {
                var response = await Client.PostAsJsonAsync(path, payload);
                response.EnsureSuccessStatusCode();
                TResult result;
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<TResult>();
                }
                else
                {
                    var errorMessage = $"Unsuccess Response for: {ROOT_URI}{path}";
                    errorMessage += $"\nstatus code: {response.StatusCode.ToString()}";
                    throw (new Exception(errorMessage));
                }
                return result;
            }
            catch (WebException webExcp)
            {
                var errorMessage = $"WebException has been caught  for: {ROOT_URI}{path}";
                errorMessage += $"\nwebExcp: {webExcp}";

                var status = webExcp.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    errorMessage += $"\nThe server returned protocol error ";
                    var httpResponse = (HttpWebResponse)webExcp.Response;
                    errorMessage += $"\n{(int)httpResponse.StatusCode} - {httpResponse.StatusCode}";
                }
                throw (new Exception(errorMessage));
            }
        }
    }
}
