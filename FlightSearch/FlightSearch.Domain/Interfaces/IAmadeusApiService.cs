using FlightSearch.Domain.Entities;

namespace FlightSearch.Domain.Interfaces;

public interface IAmadeusApiService
{
    Task<HttpResponseMessage> GetFlightsAsync(FlightsSearchParameters parameters);
}