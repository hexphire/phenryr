using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Phenryr.Services
{
    public class ApiService
    {

        public static HttpClient HttpApiClient { get; set; }
        public static HttpClient EftApiClient { get; set; }

        public static void InitializeClient()
        {
            HttpApiClient = new HttpClient();
            HttpApiClient.DefaultRequestHeaders.Accept.Clear();
            HttpApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            EftApiClient = new HttpClient();
            EftApiClient.DefaultRequestHeaders.Accept.Clear();
            EftApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


  

    }
}
