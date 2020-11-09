using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using ResourceIdeaUI.Shared.Models;
using ResourceIdeaUI.Web.Services;

namespace ResourceIdeaUI.Web.Pages.DepartmentPages
{
    public partial class DepartmentPage
    {
        private bool loading;
        private IEnumerable<Department> departments;

        [Inject]
        public IDepartmentService DepartmentService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            loading = true;
            departments = await DepartmentService.GetDepartments();
            loading = false;
        }
    }
}
