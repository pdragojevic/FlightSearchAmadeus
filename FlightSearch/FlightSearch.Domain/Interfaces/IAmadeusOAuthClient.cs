namespace FlightSearch.Domain.Interfaces;

public interface IAmadeusOAuthClient
{
    Task<string> GetAccessTokenAsync();
}