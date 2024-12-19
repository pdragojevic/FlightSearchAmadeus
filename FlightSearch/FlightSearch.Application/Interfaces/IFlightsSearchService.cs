using FlightSearch.Application.Models;
using FlightSearch.Domain.Entities;

namespace FlightSearch.Application.Interfaces;

public interface IFlightsSearchService
{
    Task<List<FlightsSearchResult>> GetFlights(FlightsSearchParameters parameters);
}