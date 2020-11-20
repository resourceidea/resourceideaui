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
        private Guid departmentId;

        [Inject]
        public IDepartmentService DepartmentService { get; set; }

        [Parameter]
        public string Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(Id))
                departmentId = await Task.Run(() => Guid.Parse(Id));
        }
    }
}
