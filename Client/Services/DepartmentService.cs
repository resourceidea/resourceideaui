using Client.Models.DataModels;
using Client.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface IDepartmentService
    {
        Task<DepartmentsListResponse> GetDepartments(string page = null);
        Task<Department> AddDepartment(Department department);
        Task<Department> GetDepartmentById(Guid id);
    }

    public class DepartmentService : IDepartmentService
    {
        private IHttpService _httpService;

        private bool IsANumber(string numberString)
        {
            return int.TryParse(numberString, out int numberValue) && numberValue > 0;
        }

        public DepartmentService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<DepartmentsListResponse> GetDepartments(string page = null)
        {
            string queryPage = string.Empty;
            if (page != null && !string.IsNullOrEmpty(page) && IsANumber(page))
            {
                queryPage = $"?page={page}";
            }
            return await _httpService.Get<DepartmentsListResponse>($"/api/departments/{queryPage}");
        }

        public async Task<Department> AddDepartment(Department department)
        {
            return await _httpService.Post<Department>("/api/departments/", department);
        }

        public async Task<Department> GetDepartmentById(Guid id)
        {
            return await _httpService.Get<Department>($"/departments/{id}");
        }
    }
}
