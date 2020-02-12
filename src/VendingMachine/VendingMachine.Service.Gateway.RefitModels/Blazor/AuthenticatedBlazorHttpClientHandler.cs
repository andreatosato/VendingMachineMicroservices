using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VendingMachine.Service.Gateway.RefitModels
{
    public class AuthenticatedBlazorHttpClientHandler : WebAssembly.Net.Http.HttpClient.WasmHttpMessageHandler // DelegatingHandler
    {
        private readonly Func<Task<string>> getToken;

        public AuthenticatedBlazorHttpClientHandler(Func<Task<string>> getToken)
        {
            this.getToken = getToken ?? throw new ArgumentNullException(nameof(getToken));
            //this.SslProtocols = System.Security.Authentication.SslProtocols.Tls
            //    | System.Security.Authentication.SslProtocols.Tls11
            //    | System.Security.Authentication.SslProtocols.Tls12;
            //this.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;
        }

        public Version Version { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await getToken().ConfigureAwait(false);
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
            if (request.RequestUri.AbsoluteUri.Contains("/product-api/Products"))
            {
                request.Headers.Add("x-api-version", "2.0");
            }
            
            request.Version = Version;
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return response;            
        }
    }
}
