using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace UdemyMicroservices.Gateway.DelegateHandlers
{
    public class TokenExchangeDelegateHandler : DelegatingHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _accessToken;

        public TokenExchangeDelegateHandler(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        private async Task<string> GetToken(string requestToken)
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                return _accessToken;
            }

            var discover = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _configuration["IdentityServerURL"],
                Policy = new DiscoveryPolicy()
                {
                    RequireHttps = false
                }
            });

            if (discover.IsError)
            {
                throw discover.Exception;
            }

            TokenExchangeTokenRequest tokenExchangeTokenRequest = new TokenExchangeTokenRequest()
            {
                Address = discover.TokenEndpoint,
                ClientId = _configuration["ClientId"],
                ClientSecret = _configuration["ClientSecret"],
                GrantType = _configuration["TokenGrantType"],
                SubjectToken = requestToken,
                SubjectTokenType = "urn:ietf:params:oauth:token-type:access-token",
                Scope = "openid discount_fullpermission fake_payment_fullpermission"
            };

            var tokenResponse = await _httpClient.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest);

            if (tokenResponse.IsError)
            {
                throw tokenResponse.Exception;
            }

            _accessToken = tokenResponse.AccessToken;

            return _accessToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestToken = request.Headers.Authorization.Parameter;

            var newToken = await GetToken(requestToken);

            request.SetBearerToken(newToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
