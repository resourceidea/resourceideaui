using System.Threading.Tasks;
using ResourceIdeaUI.Shared.Models;
using ResourceIdeaUI.Shared.ResponseModels;

namespace ResourceIdeaUI.Web.Services
{
    public interface IDepartmentService
    {
        Task<DepartmentsListResponse> GetDepartmentsAsync();
        Task<Department> AddDepartmentAsync(Department department);
    }

    public class DepartmentService : IDepartmentService
    {
        private IHttpService _httpService;

        public DepartmentService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<DepartmentsListResponse> GetDepartmentsAsync()
        {
            return await _httpService.Get<DepartmentsListResponse>("/departments/");
        }

        public async Task<Department> AddDepartmentAsync(Department department)
        {
            return await _httpService.Post<Department>("/departments/", department);
        }
    }
}
