using System.Net.Http.Json;
using Geo.Nominatim.Models;

namespace Geo.Nominatim;

/// <summary>
/// NominatimApi is a wrapper for the Nominatim geocoding API, which provides 
/// the ability to geocode addresses and reverse geocode coordinates.
/// Note that Nominatim is a service provided by OpenStreetMap, and it has usage policies which must be followed.
/// See <see href="https://nominatim.org"/>
/// <see href="https://operations.osmfoundation.org/policies/nominatim/"/>
/// </summary>
/// <remarks>
/// Nominatim requirements:
/// <list type="bullet">
/// <item>Your app can make an absolute max of 1 request per second.</item>
/// <item>Your app must cache the results.</item>
/// <item>Your app must be identified by a user agent header.</item>
/// </list>
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient"/>
public class NominatimService
{
    private IHttpClientFactory? _httpClientFactory;
    private static HttpClient? _httpClient;

    /// <summary>   
    /// Gets the HttpClient instance for making requests to the Nominatim API.
    /// If using from a web app, see <see cref="NominatimService(IHttpClientFactory)"/>.
    /// Otherwise, see <see cref="NominatimService(string, string)"/>.
    /// </summary>
    internal HttpClient Client
    {
        get
        {
            if (_httpClientFactory != null)
            {
                // If using IHttpClientFactory, as from a web app, create a new client instance
                return _httpClientFactory.CreateClient("Nominatim");
            }

            // If using a static HttpClient, return the existing instance
            if (_httpClient != null)
            {
                return _httpClient;
            }

            throw new InvalidOperationException("Nominatim is not properly initialized.");
        }
    }

    /// <summary>
    /// Creates an instance of the NominatimApi class for running from a web app. (uses an IHttpClientFactory)
    /// </summary>
    /// <example>
    /// In Program.cs:
    /// <code>
    /// builder.Services.AddHttpClient("Nominatim", httpClient =>
    /// {
    ///     httpClient.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
    ///     // user agent header is required by Nominatim
    ///     httpClient.DefaultRequestHeaders.Add("User-Agent", "EmpMap/0.1");
    /// });
    /// builder.Services.AddScoped<NominatimService>();
    /// </code>
    /// </example>
    /// <param name="httpClientFactory"></param>
    public NominatimService(IHttpClientFactory httpClientFactory) =>
        _httpClientFactory = httpClientFactory;

    /// <summary>
    /// Creates an instance of the NominatimApi class for running in a server-side application. (uses a static HttpClient)
    /// </summary>
    /// <param name="userAgent">The user agent to set in the HttpClient headers.</param>
    public NominatimService(string userAgent, string baseAddressUrl = "https://nominatim.openstreetmap.org/")
    {
        if (_httpClient != null)
            return;

        // Initialize the static HttpClient instance
        _httpClient = new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(30),
            PooledConnectionIdleTimeout = TimeSpan.FromSeconds(10)
        })
        {
            BaseAddress = new Uri(baseAddressUrl),
            DefaultRequestHeaders =
            { // Nominatim requires a user agent header
                { "User-Agent", userAgent }
            },
        };
    }

    /// <summary>
    /// Checks the status of the Nominatim API and returns the text response.
    /// </summary>
    /// <returns>The response content as a string.</returns>
    public async Task<string?> GetStastusAsync()
    {
        var response = await Client.GetAsync("status");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Get the location info for a given address.
    /// </summary>
    /// <returns>The response content as a SearchResult?</returns>
    /// <seealso href="https://nominatim.org/release-docs/develop/api/Search/"/>
    public async Task<SearchResult?> GeocodeAsync(string address)
    {
        return await Client.GetFromJsonAsync<SearchResult>(
            $"search?q={address}&format=geocodejson&addressdetails=1&limit=1");
    }

    /// <summary>
    /// Get the location info for a given city and state.
    /// </summary>
    /// <returns>The response content as a SearchResult?</returns>
    /// <seealso href="https://nominatim.org/release-docs/develop/api/Search/"/>
    public async Task<SearchResult?> GeocodeByCityStateAsync(string city, string state)
    {
        return await Client.GetFromJsonAsync<SearchResult>(
            $"search?state={state}&city={city}&format=geocodejson&addressdetails=1&limit=1");
    }

    /// <summary>
    /// Get the address info for a given lat/lon.
    /// </summary>
    /// <returns>The response content as a dynamic (json)</returns>
    public async Task<dynamic?> ReverseGeocodeAsync(string lat, string lon)
    {
        return await Client.GetFromJsonAsync<dynamic>(
            $"reverse?lat={lat}&lon={lon}&format=geocodejson&addressdetails=1&limit=1");
    }
}

