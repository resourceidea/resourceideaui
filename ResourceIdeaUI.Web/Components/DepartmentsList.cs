using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using ResourceIdeaUI.Shared.Models;
using ResourceIdeaUI.Shared.ResponseModels;
using ResourceIdeaUI.Web.Services;

namespace ResourceIdeaUI.Web.Components
{
    public partial class DepartmentsList : IDisposable
    {
        private bool loading;
        private List<Department> departments = new List<Department>();
        private bool disablePrevious;
        private bool disableNext;
        private string previousPage;
        private string nextPage;

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
            DepartmentsListResponse departmentsQueryResponse = await DepartmentService.GetDepartmentsAsync();
            
            previousPage = departmentsQueryResponse.Previous;
            nextPage = departmentsQueryResponse.Next;

            DisableOrEnablePreviousPageLink(previousPage);
            DisableOrEnableNextPageLink(nextPage);

            departments.AddRange(departmentsQueryResponse.Results);
            
            departments.Sort((x, y) => string.Compare(x.Name, y.Name));
            loading = false;
            
            NewDepartmentNotifier.Notify += OnNotify;
        }

        private void DisableOrEnablePreviousPageLink(string previousPage)
        {
            if (string.IsNullOrEmpty(previousPage))
                disablePrevious = true;
            else
                disablePrevious = false;
        }

        private void DisableOrEnableNextPageLink(string nextPage)
        {
            if (string.IsNullOrEmpty(nextPage))
                disableNext = true;
            else
                disableNext = false;
        }

        private void HandlePreviousPage()
        {
            throw new NotImplementedException();
        }

        private void HandleNextPage()
        {
            throw new NotImplementedException();
        }
    }
}
