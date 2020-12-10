using Microsoft.AspNetCore.Components;
using ResourceIdeaUI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceIdeaUI.Web.Services
{
    public class NewDepartmentNotifierService
    {
        private readonly List<Department> departments = new List<Department>();
        public IReadOnlyList<Department> DepartmentsList => departments;

        public NewDepartmentNotifierService()
        {
        }

        public event Func<Task> Notify;

        public async Task AddToListAsync(Department department)
        {
            departments.Add(department);
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
