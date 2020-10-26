using ResourceIdeaUI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceIdeaUI.Services
{
    public interface IAuthenticationService
    {
        Token Token { get; }

        public Task Initialize();

        Task Login(string username, string password);

        Task Logout();
    }
}
