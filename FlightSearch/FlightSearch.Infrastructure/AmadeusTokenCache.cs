using FlightSearch.Domain.Interfaces;

namespace FlightSearch.Infrastructure;

public class AmadeusTokenCache : IAmadeusTokenCache
{
    private string? _accessToken;
    private DateTime _expiration;

    public string? GetToken()
    {
        if (_accessToken != null && _expiration > DateTime.UtcNow)
        {
            return _accessToken;
        }
        return null;
    }

    public void SetToken(string token, int expiresIn)
    {
        _accessToken = token;
        _expiration = DateTime.UtcNow.AddSeconds(expiresIn);
    }
}