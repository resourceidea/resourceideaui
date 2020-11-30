using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using ResourceIdeaUI.Shared.Models;
using ResourceIdeaUI.Web.Pages.DepartmentPages;
using ResourceIdeaUI.Web.Services;

namespace ResourceIdeaUI.Web.Components
{
    public partial class AddDepartment
    {
        private Department department = new Department();
        private Token token;

        [Inject]
        public IDepartmentService DepartmentService { get; set; }

        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public EventCallback OnDepartmentAdded { get; set; }

        protected override async void OnInitialized()
        {
            token = await LocalStorageService.GetItem<Token>("token");
        }

        private void HandleValidSubmit()
        {
            
        }
    }
}
