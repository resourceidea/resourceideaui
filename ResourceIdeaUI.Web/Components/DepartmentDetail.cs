using Microsoft.AspNetCore.Components;
using ResourceIdeaUI.Shared.Models;
using ResourceIdeaUI.Web.Services;
using System;
using System.Threading.Tasks;

namespace ResourceIdeaUI.Web.Components
{
    public partial class DepartmentDetail
    {
        private Department department = new Department();

        [Inject] 
        public IDepartmentService DepartmentService { get; set; }

        [Parameter] 
        public Guid DepartmentId { get; set; }

        protected async override Task OnInitializedAsync()
        {
            department = await DepartmentService.GetDepartmentById(DepartmentId);
        }
    }
}
