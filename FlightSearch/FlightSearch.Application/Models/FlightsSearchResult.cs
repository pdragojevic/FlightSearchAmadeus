namespace FlightSearch.Application.Models;

public class FlightsSearchResult
{
    public string OriginLocationCode { get; set; }
    public string DestinationLocationCode { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int DepartureTransfers { get; set; }
    public int? ReturnTransfers { get; set; }
    public int Passengers { get; set; }
    public string CurrencyCode { get; set; }
    public decimal Price { get; set; }
}