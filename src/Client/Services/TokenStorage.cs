using Microsoft.JSInterop;

namespace EastSeat.ResourceIdea.Client.Services;

public sealed class TokenStorage
{
    private readonly IJSRuntime _js;
    private const string TokenKey = "resourceidea_token";

    public TokenStorage(IJSRuntime js) => _js = js;

    public async Task SetTokenAsync(string? token)
    {
        if (token == null)
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        }
        else
        {
            await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        }
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
    }
}