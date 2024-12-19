using System.Net.Http.Headers;
using System.Text;
using FlightSearch.Domain.Entities;
using FlightSearch.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace FlightSearch.Infrastructure;

public class AmadeusApiService : IAmadeusApiService
{
    private readonly IOptions<AmadeusApiSettings> _apiSettings;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAmadeusOAuthClient _oauthClient;

    public AmadeusApiService(IOptions<AmadeusApiSettings> apiSettings, IHttpClientFactory httpClient, IAmadeusOAuthClient oauthClient)
    {
        _apiSettings = apiSettings;
        _httpClientFactory = httpClient;
        _oauthClient = oauthClient;
    }

    public async Task<HttpResponseMessage> GetFlightsAsync(FlightsSearchParameters parameters)
    {
        var baseUrl = _apiSettings.Value.BaseUrl;
        var endpoint = _apiSettings.Value.EndpointFlightOffer;
        
        var accessToken = await _oauthClient.GetAccessTokenAsync();
        
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(baseUrl);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var queryString = BuildQueryString(parameters);
        var request = new HttpRequestMessage(HttpMethod.Get, $"{endpoint}?{queryString}");
        
        return await httpClient.SendAsync(request);
    }
    
    private string BuildQueryString(FlightsSearchParameters parameters)
    {
        var queryString = new StringBuilder();

        queryString.Append($"originLocationCode={parameters.OriginLocationCode}");
        queryString.Append($"&destinationLocationCode={parameters.DestinationLocationCode}");
        queryString.Append($"&departureDate={parameters.DepartureDate.ToString("yyyy-MM-dd")}");

        if (parameters.ReturnDate != null)
        {
            queryString.Append($"&returnDate={parameters.ReturnDate.Value.ToString("yyyy-MM-dd")}");
        }
           
        queryString.Append($"&adults={parameters.Adults}");
        queryString.Append($"&currencyCode={parameters.CurrencyCode}");

        return queryString.ToString();
    }
}