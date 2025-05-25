using System.Text.Json.Serialization;
namespace Geo.Nominatim.Models;

/// <summary>
/// A component of a Nominatim geojson result found as a list under the root object.
/// </summary>
/// <exmaple>
/// <code>
/// {
///    "type": "Feature",
///    "properties": {},
///    "geometry": { }
/// }
/// </code>
/// </exmaple>
public record Feature
{

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("properties")]
    public Properties? Properties { get; set; }

    [JsonPropertyName("geometry")]
    public Geometry? Geometry { get; set; }
}
