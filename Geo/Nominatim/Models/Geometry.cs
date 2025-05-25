using System.Text.Json.Serialization;
namespace Geo.Nominatim.Models;

/// <summary>
/// A component of a Nominatim geojson result
/// </summary>
/// <exmaple>
/// <code>
/// "geometry": {
///"type": "Point",
///    "coordinates": [
///      -96.71470556756292,
///      32.96309615
///    ]
///  }
/// </code>
/// </exmaple>
public record Geometry
{

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("coordinates")]
    public List<double>? Coordinates { get; set; }
}