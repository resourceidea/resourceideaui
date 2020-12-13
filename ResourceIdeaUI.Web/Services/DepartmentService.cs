using System.Threading.Tasks;
using ResourceIdeaUI.Shared.Models;
using ResourceIdeaUI.Shared.ResponseModels;

namespace ResourceIdeaUI.Web.Services
{
    public interface IDepartmentService
    {
        Task<DepartmentsListResponse> GetDepartmentsAsync(string page=null);
        Task<Department> AddDepartmentAsync(Department department);
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

        public async Task<DepartmentsListResponse> GetDepartmentsAsync(string page=null)
        {
            string queryPage = string.Empty;
            if (page != null && !string.IsNullOrEmpty(page) && IsANumber(page))
            {
                queryPage = $"?page={page}";
            }
            return await _httpService.Get<DepartmentsListResponse>($"/departments/{queryPage}");
        }

        public async Task<Department> AddDepartmentAsync(Department department)
        {
            return await _httpService.Post<Department>("/departments/", department);
        }
    }
}
