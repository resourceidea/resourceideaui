using ResourceIdeaUI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ResourceIdeaUI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public Token Token { get; private set; }

        public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task Initialize()
        {
            Token = await _localStorageService.GetItem<Token>("token");
        }

        public async Task Login(string username, string password)
        {
            var user = new User { username = username, password = password };
            var loginJson = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/token/", loginJson);

            if (response.IsSuccessStatusCode)
            {
                var token = await JsonSerializer.DeserializeAsync<Token>(await response.Content.ReadAsStreamAsync());
                await _localStorageService.SetItem("token", token);

                await Initialize();
            }
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItem("token");
            await Initialize();
        }
    }
}
