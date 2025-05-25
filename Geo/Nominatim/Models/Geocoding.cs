using System.Text.Json.Serialization;

namespace Geo.Nominatim.Models;
/// <summary>
/// Two different components of a Nominatim geojson result.
/// </summary>
/// <example>
/// <code>
/// {
///   "version": "0.1.0",
///   "attribution": "Data © OpenStreetMap contributors, ODbL 1.0. http://osm.org/copyright",
///   "licence": "ODbL",
///   "query": "1201 N Bowser Rd, Richardson, TX"
/// }
/// </code>
/// </example>
/// <example>
/// <code>
/// {
///   "place_id": 310613274,
///   "osm_type": "way",
///   "osm_id": 597876764,
///   "osm_key": "building",
///   "osm_value": "commercial",
///   "type": "house",
///   "label": "1201, North Bowser Road, Richardson, Dallas County, Texas, 75081, United States",
///   "housenumber": "1201",
///   "postcode": "75081",
///   "street": "North Bowser Road",
///   "city": "Richardson",
///   "county": "Dallas County",
///   "state": "Texas",
///   "country": "United States",
///   "country_code": "us",
///   "admin": {
///     "level8": "Richardson",
///     "level6": "Dallas County",
///     "level4": "Texas"
///   }
/// }
/// </code>
/// </example>
public record Geocoding
{
    // Object style 1
    // Found under the root object

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("attribution")]
    public string? Attribution { get; set; }

    [JsonPropertyName("license")]
    public string? Licence { get; set; }

    [JsonPropertyName("query")]
    public string? Query { get; set; }

    // Object syle 2
    // Found under root > properties
    [JsonPropertyName("place_id")]
    public int? PlaceId { get; set; }

    [JsonPropertyName("osm_type")]
    public string? OsmType { get; set; }

    [JsonPropertyName("osm_id")]
    public int? OsmId { get; set; }

    [JsonPropertyName("osm_key")]
    public string? OsmKey { get; set; }

    [JsonPropertyName("osm_value")]
    public string? OsmValue { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("housenumber")]
    public string? HouseNumber { get; set; }

    [JsonPropertyName("postcode")]
    public string? Postcode { get; set; }

    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("vity")]
    public string? City { get; set; }

    [JsonPropertyName("vounty")]
    public string? County { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("country_code")]
    public string? CountryCode { get; set; }
}