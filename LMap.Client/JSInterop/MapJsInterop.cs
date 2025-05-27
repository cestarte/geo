using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
namespace LMap.Client.JSInterop;

/// <summary>
/// Wrap the leaflet JavaScript 1.x API for use in Blazor components.
///
/// The associated JavaScript module is loaded on demand when first needed. 
/// This class can be registered as scoped DI service and then injected into Blazor
/// components for use.
/// </summary>
public class MapJSInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;
    public IJSObjectReference? Map { get; set; }
    public List<IJSObjectReference> Markers { get; set; } = new();

    public MapJSInterop(IJSRuntime jsRuntime)
    {
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./map.js").AsTask());
    }

    public async ValueTask<string> Prompt(string message)
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<string>("showPrompt", message);
    }

    public async ValueTask<IJSObjectReference> InitMap(ElementReference elementRef)
    {
        var module = await moduleTask.Value;
        var map = await module.InvokeAsync<IJSObjectReference>("initMap", elementRef);
        Map = map;
        return map;
    }

    public async ValueTask<IJSObjectReference> AddMarker(double latitude, double longitude, string popupText)
    {
        var module = await moduleTask.Value;
        var marker = await module.InvokeAsync<IJSObjectReference>("addMarker", Map, latitude, longitude, popupText);
        Markers.Add(marker);
        return marker;
    }

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
