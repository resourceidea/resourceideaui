using Client.Models.DataModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface IAuthenticationService
    {
        User User { get; }
        Token Token { get; }
        Task Initialize();
        Task Login(string username, string password);
        Task Logout();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private IHttpService _httpService;
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;

        public User User { get; private set; }
        public Token Token { get; private set; }

        public AuthenticationService(
            IHttpService httpService,
            NavigationManager navigationManager,
            ILocalStorageService localStorageService)
        {
            _httpService = httpService;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }

        public async Task Initialize()
        {
            Token = await _localStorageService.GetItem<Token>("token");
        }

        public async Task Login(string username, string password)
        {
            Token = await _httpService.Post<Token>("/api/token/", new { username, password });
            await _localStorageService.SetItem("token", Token);

            KeepSession();
        }

        public async Task Logout()
        {
            Token = null;
            await _localStorageService.RemoveItem("token");
            _navigationManager.NavigateTo("login");
        }

        public async void KeepSession()
        {
            while (Token != null)
            {
                await Task.Delay(60000);
                string refresh = Token.refresh;
                var refreshToken = await _httpService.Post<Token>("/api/token/refresh/", new { refresh });
                Token.access = refreshToken.access;
                await _localStorageService.SetItem("token", Token);
            }
        }
    }
}
