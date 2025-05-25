using System.Text.Json.Serialization;
namespace Geo.Nominatim.Models;

/// <summary>
/// A component of a Nominatim geojson result found under the root object.
/// </summary>
/// <example>
/// <code>
/// {
/// "geocoding": {
///   }
/// }
/// </code>
/// </example>
public record Properties
{

    [JsonPropertyName("geocoding")]
    public Geocoding? Geocoding { get; set; }
}