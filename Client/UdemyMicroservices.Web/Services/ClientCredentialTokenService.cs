using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UdemyMicroservices.Web.Models;
using UdemyMicroservices.Web.Services.Interfaces;

namespace UdemyMicroservices.Web.Services
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ClientSettings _clientSettings;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;
        private readonly HttpClient _httpClient;

        public ClientCredentialTokenService(IOptions<ServiceApiSettings> serviceApiSettings, IOptions<ClientSettings> clientSettings, IClientAccessTokenCache clientAccessTokenCache, HttpClient httpClient)
        {
            _serviceApiSettings = serviceApiSettings.Value ?? throw new ArgumentNullException(nameof(serviceApiSettings));
            _clientSettings = clientSettings.Value ?? throw new ArgumentNullException(nameof(clientSettings));
            _clientAccessTokenCache = clientAccessTokenCache ?? throw new ArgumentNullException(nameof(clientAccessTokenCache));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> GetToken()
        {
            var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken");

            if (currentToken != null)
            {
                return currentToken.AccessToken;
            }

            var discover = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy()
                {
                    RequireHttps = false
                }
            });

            if (discover.IsError)
            {
                throw discover.Exception;
            }

            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest()
            {
                ClientId = _clientSettings.WebMvcClient.ClientId,
                ClientSecret = _clientSettings.WebMvcClient.ClientSecret,
                Address = discover.TokenEndpoint
            };

            var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);

            if (newToken.IsError)
            {
                throw newToken.Exception;
            }

            await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken, newToken.ExpiresIn);

            return newToken.AccessToken;
        }
    }
}
