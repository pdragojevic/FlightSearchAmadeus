using FlightSearch.Application.Interfaces;
using FlightSearch.Application.Models;
using FlightSearch.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace FlightSearch.Application.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _cache;
    private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(5);
    
    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<string?> GetCachedResultAsync(FlightsSearchParameters parameters)
    {
        var cashKey = GenerateHash(parameters);
        return await _cache.GetStringAsync(cashKey);
    }

    public async Task SaveSearchResultAsync(FlightsSearchParameters parameters, List<FlightsSearchResult> result)
    {
        var cashKey = GenerateHash(parameters);
        await _cache.SetStringAsync(
            cashKey, 
            JsonConvert.SerializeObject(result),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheExpiry }
        );
    }

    private string GenerateHash(FlightsSearchParameters parameters)
    {
        var departureDate = parameters.DepartureDate.ToString("yyyy-MM-dd");
        var returnDate = parameters.ReturnDate?.ToString("yyyy-MM-dd") ?? string.Empty;
        var cache = new
        {
            parameters.OriginLocationCode,
            parameters.DestinationLocationCode,
            departureDate,
            returnDate,
            parameters.Adults,
            parameters.CurrencyCode
        };
        return cache.GetHashCode().ToString();
    }
}