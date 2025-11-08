using System.Net.Http.Json;
using EastSeat.ResourceIdea.Shared.Models;

namespace EastSeat.ResourceIdea.Client.Services;

public sealed class AuthService
{
    private readonly HttpClient _http;
    private readonly TokenStorage _storage;

    public AuthService(HttpClient http, TokenStorage storage)
    {
        _http = http;
        _storage = storage;
    }

    public async Task<bool> RegisterAsync(RegisterRequest req)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", req);
        return response.IsSuccessStatusCode;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest req)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", req);
        if (!response.IsSuccessStatusCode) return null;
        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (auth != null)
        {
            await _storage.SetTokenAsync(auth.AccessToken);
        }
        return auth;
    }

    public async Task LogoutAsync()
    {
        await _storage.SetTokenAsync(null);
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest req)
    {
        var response = await _http.PostAsJsonAsync("api/auth/forgot-password", req);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordRequest req)
    {
        var response = await _http.PostAsJsonAsync("api/auth/reset-password", req);
        return response.IsSuccessStatusCode;
    }
}
