using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using ResourceIdeaUI.Shared.Models;
using ResourceIdeaUI.Web.Services;

namespace ResourceIdeaUI.Web.Components
{
    public partial class DepartmentsList : IDisposable
    {
        private bool loading;
        private List<Department> departments = new List<Department>();

        [Inject]
        public IDepartmentService DepartmentService { get; set; }

        [Inject]
        public NewDepartmentNotifierService NewDepartmentNotifier { get; set; }

        [Parameter]
        public string Id { get; set; }

        public async Task OnNotify()
        {
            await InvokeAsync(() =>
            {
                departments.AddRange(NewDepartmentNotifier.DepartmentsList);
                departments.Sort((x, y) => string.Compare(x.Name, y.Name));
                StateHasChanged();
            });
        }

        public void Dispose()
        {
            NewDepartmentNotifier.Notify -= OnNotify;
        }

        protected override async Task OnInitializedAsync()
        {
            loading = true;
            departments.AddRange((await DepartmentService.GetDepartmentsAsync()));
            departments.Sort((x, y) => string.Compare(x.Name, y.Name));
            loading = false;
            NewDepartmentNotifier.Notify += OnNotify;
        }
    }
}
