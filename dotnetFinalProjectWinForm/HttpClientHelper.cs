using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace dotnetFinalProjectWinForm
{
    internal class HttpClientHelper
    {
        public static readonly HttpClient httpClient = new HttpClient();
        public static HttpClient GetClient() { return httpClient; }

    }
}
