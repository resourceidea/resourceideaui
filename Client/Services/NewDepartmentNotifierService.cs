using Client.Helpers;
using Client.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services
{
    public class NewDepartmentNotifierService
    {
        private readonly List<Department> departments = new List<Department>();
        public IReadOnlyList<Department> DepartmentsList => departments;
        public string PreviousPage { get; private set; }
        public string NextPage { get; private set; }
        private readonly IDepartmentService _departmentService;

        public NewDepartmentNotifierService(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public event Func<Task> Notify;

        public async Task UpdateListAsync(string page = null)
        {
            var departmentsQuery = await _departmentService.GetDepartments(page);
            PreviousPage = departmentsQuery.Previous.GetPageNumber();
            NextPage = departmentsQuery.Next.GetPageNumber();
            departments.Clear();
            departments.AddRange(departmentsQuery.Results);

            if (Notify != null)
            {
                await Notify?.Invoke();
            }
        }

        public async Task ClearListAsync()
        {
            departments.Clear();
            if (Notify != null)
            {
                await Notify?.Invoke();
            }
        }
    }
}
