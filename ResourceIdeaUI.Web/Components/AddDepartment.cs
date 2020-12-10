using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ResourceIdeaUI.Shared.Models;
using ResourceIdeaUI.Web.Pages.DepartmentPages;
using ResourceIdeaUI.Web.Services;

namespace ResourceIdeaUI.Web.Components
{
    public partial class AddDepartment
    {
        private string DepartmentName { get; set; }

        [Inject]
        public IDepartmentService DepartmentService { get; set; }

        [Inject]
        public NewDepartmentNotifierService Notifier { get; set; }

        private async void HandleValidSubmit()
        {
            var department = new Department { Name = DepartmentName };
            var newDepartment = await DepartmentService.AddDepartmentAsync(department);
            await Notifier.AddToListAsync(newDepartment);
        }
    }
}
