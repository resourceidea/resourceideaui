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

        public event Func<Task> Notify;

        public NewDepartmentNotifierService()
        {
        }

        public async Task AddToList(Department department)
        {
            departments.Add(department);
            if (Notify != null)
            {
                await Notify?.Invoke();
            }
        }
    }
}
