using FlightSearch.Application.Interfaces;
using FlightSearch.Application.Models;
using FlightSearch.Domain.Entities;
using FlightSearch.Domain.Interfaces;
using Newtonsoft.Json;

namespace FlightSearch.Application.Services;

public class FlightsSearchService : IFlightsSearchService
{
    private readonly IRedisCacheService _redisCacheService;
    private readonly IAmadeusApiService _apiService;

    public FlightsSearchService(IRedisCacheService redisCacheService, IAmadeusApiService apiService)
    {
        _redisCacheService = redisCacheService;
        _apiService = apiService;
    }

    public async Task<List<FlightsSearchResult>> GetFlights(FlightsSearchParameters parameters)
    {
        var cachedResult = await _redisCacheService.GetCachedResultAsync(parameters);
        if (cachedResult != null)
        {
            return JsonConvert.DeserializeObject<List<FlightsSearchResult>>(cachedResult)!;
        }
        
        var apiResult = await _apiService.GetFlightsAsync(parameters);
        var flightOffersResponse = JsonConvert.DeserializeObject<FlightsSearchApiResponse>(apiResult.Content.ReadAsStringAsync().Result);
        
        var results = new List<FlightsSearchResult>();
        
        if (flightOffersResponse.Meta.Count == 0)
        {
            return results;
        }
        
        foreach (var flightOffer in flightOffersResponse.Data)
        {
            var departureFlightData = flightOffer.Itineraries.FirstOrDefault();
            var returnFlightData = flightOffer.Itineraries.Skip(1).FirstOrDefault();

            var model = new FlightsSearchResult
            {
                OriginLocationCode = parameters.OriginLocationCode,
                DestinationLocationCode = parameters.DestinationLocationCode,
                DepartureDate = departureFlightData.Segments.FirstOrDefault().Departure.At,
                ReturnDate = returnFlightData?.Segments.FirstOrDefault()?.Departure.At,
                DepartureTransfers = (departureFlightData?.Segments.Length ?? 1) - 1,
                ReturnTransfers = (returnFlightData?.Segments.Length ?? 1) - 1,
                Passengers = parameters.Adults,
                CurrencyCode = flightOffer.Price.Currency,
                Price = flightOffer.Price.GrandTotal
            };

            results.Add(model);
        }
        
        await _redisCacheService.SaveSearchResultAsync(parameters, results);
        
        return results;
    }
}