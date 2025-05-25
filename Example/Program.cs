using Geo.Nominatim;
using Geo.Nominatim.Models;

List<City> citiesWithoutGeo = new List<City>
{
    new City { Name = "Las Cruces", State = "NM" },
    new City { Name = "Brownsville", State = "TX" },
    new City { Name = "Miami", State = "FL" },
};

List<City> geocodedCities = await GeocodeCitiesWithNominatim(citiesWithoutGeo);

Console.WriteLine("Geocoded Locations:");
foreach (var place in geocodedCities)
{
    Console.WriteLine($"{place.Name}, {place.State}: {place.Latitude}, {place.Longitude}");
}

/// <summary>
/// Extracts coordinate from a Nominatim search result.
/// </summary>
/// <param name="searchResult">The search result from the Nominatim API.</param>
static (double? lat, double? lon, string msg) GetCoordFromResult(SearchResult? result)
{
    if (result == null || result.Features == null)
    {
        return (null, null, "Nothing found.");
    }
    // for this example, we only care about the first feature
    var firstFeature = result.Features.FirstOrDefault();
    if (firstFeature == null || firstFeature.Geometry == null)
    {
        return (null, null, "No feature geometry found.");
    }
    var coords = firstFeature.Geometry.Coordinates;
    if (coords == null || coords.Count < 2)
    {
        return (null, null, $"Invalid coordinates: {string.Join(", ", coords ?? new List<double>())}.");
    }

    // Nominatim returns coordinates as [longitude, latitude]
    return (coords[1], coords[0], "OK");
}

/// <summary>
/// Calls the Nominatim geocoder for each location on the list, and delays between requests to honor the rate limit.
/// </summary>
static async Task<List<City>> GeocodeCitiesWithNominatim(List<City> cities)
{
    var geocoder = new NominatimService("Nominatim Example/0.1", "https://nominatim.openstreetmap.org");
    var geocodedCities = new List<City>();

    foreach (var city in cities)
    {
        Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] Geocoding {city.Name}, {city.State}...");
        var searchResult = await geocoder.GeocodeByCityStateAsync(city.Name, city.State);

        var (lat, lon, msg) = GetCoordFromResult(searchResult);
        if (lat == null || lon == null)
        {
            Console.WriteLine($"\t{msg}");
            continue; // Skip to the next if no valid coord found
        }

        geocodedCities.Add(new City
        {
            Name = city.Name,
            State = city.State,
            Latitude = lat,
            Longitude = lon,
        });
        Console.WriteLine($"\t{lat}, {lon}");

        // Rate limit to max of 1 request per second, required by Nominatim
        // https://operations.osmfoundation.org/policies/nominatim/
        await Task.Delay(3000);
    }
    return geocodedCities;
}


record City {
    public required string Name { get; set; }
    public required string State { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}