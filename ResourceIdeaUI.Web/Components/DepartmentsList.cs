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
        public NewDepartmentNotifierService Notifier { get; set; }

        [Parameter]
        public string Id { get; set; }

        public async Task OnNotify()
        {
            await InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        public void Dispose()
        {
            Notifier.Notify -= OnNotify;
        }

        protected override async Task OnInitializedAsync()
        {
            loading = true;
            await Notifier.ClearListAsync();
            DepartmentsListResponse departmentsQueryResponse = await DepartmentService.GetDepartmentsAsync();

            previousPage = departmentsQueryResponse.Previous;
            nextPage = departmentsQueryResponse.Next;

            DisableOrEnablePreviousPageLink(previousPage);
            DisableOrEnableNextPageLink(nextPage);

            foreach (var department in departmentsQueryResponse.Results)
            {
                await Notifier.AddToListAsync(department);
            }

            loading = false;

            Notifier.Notify += OnNotify;
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
