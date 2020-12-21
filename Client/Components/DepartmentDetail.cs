using Client.Models.DataModels;
using Client.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Components
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
