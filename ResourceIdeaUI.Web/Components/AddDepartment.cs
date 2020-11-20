using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using ResourceIdeaUI.Shared.Models;
using ResourceIdeaUI.Web.Services;

namespace ResourceIdeaUI.Web.Components
{
    public partial class AddDepartment
    {
        private Department department = new Department();
        private Token token;

        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            token = await LocalStorageService.GetItem<Token>("token");
            department.Organization = Guid.Empty;
        }

        private void HandleValidSubmit()
        {
            throw new NotImplementedException();
        }
    }
}
