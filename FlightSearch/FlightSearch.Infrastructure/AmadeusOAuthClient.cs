using System.Text;
using FlightSearch.Domain.Entities;
using FlightSearch.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace FlightSearch.Infrastructure;

public class AmadeusOAuthClient : IAmadeusOAuthClient
    {
        private readonly IOptions<AmadeusApiSettings> _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAmadeusTokenCache _tokenCache;

        public AmadeusOAuthClient(IOptions<AmadeusApiSettings> apiSettings, IHttpClientFactory httpClientFactory, IAmadeusTokenCache tokenCache)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
            _tokenCache = tokenCache;
        }
        public async Task<string> GetAccessTokenAsync()
        {
            var cachedToken = _tokenCache.GetToken();
            if (cachedToken != null)
            {
                return cachedToken;
            }
            var _httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, _apiSettings.Value.OauthTokenHttps)
            {
                Content = new StringContent($"grant_type=client_credentials&client_id={_apiSettings.Value.ApiKey}&client_secret={_apiSettings.Value.ApiSecret}", Encoding.UTF8, "application/x-www-form-urlencoded")
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            await using var stream = await response.Content.ReadAsStreamAsync();
            var oauthResults = await System.Text.Json.JsonSerializer.DeserializeAsync<TokenResponse>(stream);
            
            _tokenCache.SetToken(oauthResults.access_token, oauthResults.expires_in);

            return oauthResults.access_token;
        }
    }

    public class TokenResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }