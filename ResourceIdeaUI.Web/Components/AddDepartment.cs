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
        private Department department = new Department();
        private EditContext editContext;

        [Inject]
        public IDepartmentService DepartmentService { get; set; }

        [Inject]
        public NewDepartmentNotifierService NewDepartmentNotifier { get; set; }

        protected override void OnInitialized()
        {
            editContext = new EditContext(department);
        }

        private void HandleReset()
        {
            department = new Department();
            editContext = new EditContext(department);
        }

        private async void HandleValidSubmit()
        {
            var newDepartment = await DepartmentService.AddDepartmentAsync(department);
            await NewDepartmentNotifier.AddToList(newDepartment);
            department = new Department();
            StateHasChanged();
        }
    }
}
