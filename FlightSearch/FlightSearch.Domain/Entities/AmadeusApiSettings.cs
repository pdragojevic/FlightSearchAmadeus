namespace FlightSearch.Domain.Entities;

public class AmadeusApiSettings
{
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
    public string BaseUrl { get; set; }
    public string OauthTokenHttps { get; set; }
    public string EndpointFlightOffer { get; set; }
}