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

    public MapJSInterop(IJSRuntime jsRuntime)
    {
        moduleTask = new (() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./map.js").AsTask());
    }

    public async ValueTask<string> Prompt(string message)
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<string>("showPrompt", message);
    }

    public async ValueTask InitMap(ElementReference elementRef)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("initMap", elementRef);
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
