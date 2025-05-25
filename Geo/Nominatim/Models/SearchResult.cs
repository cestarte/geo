using System.Text.Json.Serialization;
namespace Geo.Nominatim.Models;

/// <summary>
/// The top level component in a Nominatim geojson result
/// </summary>
/// <example>
/// <code>
/// {
///   "type": "FeatureCollection",
///   "geocoding": {},
///   "features": []
///  }
/// </code>
/// </example>
/// <see cref="NominatimService"/>
public record SearchResult
{

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("geocoding")]
    public Geocoding? Geocoding { get; set; }

    [JsonPropertyName("features")]
    public List<Feature>? Features { get; set; }
}
