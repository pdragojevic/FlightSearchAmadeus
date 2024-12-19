using FlightSearch.Application.Models;
using FlightSearch.Domain.Entities;

namespace FlightSearch.Application.Interfaces;

public interface IRedisCacheService
{
    Task<string?> GetCachedResultAsync(FlightsSearchParameters parameters);
    Task SaveSearchResultAsync(FlightsSearchParameters parameters, List<FlightsSearchResult> result);
}