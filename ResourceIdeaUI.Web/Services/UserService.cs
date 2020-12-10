using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ResourceIdeaUI.Shared.Models;
using ResourceIdeaUI.Web.Models;

namespace ResourceIdeaUI.Web.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Organization>> GetAll();
    }

    public class UserService : IUserService
    {
        private IHttpService _httpService;

        public UserService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IEnumerable<Organization>> GetAll()
        {
            return await _httpService.Get<IEnumerable<Organization>>("/organizations/");
        }
    }
}
