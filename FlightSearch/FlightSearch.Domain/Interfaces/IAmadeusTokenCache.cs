namespace FlightSearch.Domain.Interfaces;

public interface IAmadeusTokenCache
{
    string? GetToken();
    void SetToken(string token, int expiresIn);
}