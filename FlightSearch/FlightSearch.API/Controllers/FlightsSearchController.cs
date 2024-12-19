using FlightSearch.Application.Interfaces;
using FlightSearch.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsSearchController : ControllerBase
{
    private readonly IFlightsSearchService _flightsSearchService;

    public FlightsSearchController(IFlightsSearchService flightsSearchService)
    {
        _flightsSearchService = flightsSearchService;
    }

    [HttpPost]
    public async Task<IActionResult> SearchFlights([FromBody] FlightsSearchParameters parameters)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _flightsSearchService.GetFlights(parameters);
        return Ok(result);
    }
}