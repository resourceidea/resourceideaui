using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ResourceIdeaUI.Shared.Models;

namespace ResourceIdeaUI.Web.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetDepartments();
    }

    public class DepartmentService : IDepartmentService
    {
        private IHttpService _httpService;

        public DepartmentService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            return await _httpService.Get<IEnumerable<Department>>("/api/v0.1/departments/");
        }
    }
}
