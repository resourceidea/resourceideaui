using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using ResourceIdeaUI.Shared.ExtensionMethods;
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
                DisableOrEnablePreviousPageLink(Notifier.PreviousPage);
                DisableOrEnableNextPageLink(Notifier.NextPage);

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
            previousPage = departmentsQueryResponse.Previous.GetPageNumber();
            nextPage = departmentsQueryResponse.Next.GetPageNumber();
            DisableOrEnablePreviousPageLink(previousPage);
            DisableOrEnableNextPageLink(nextPage);
            await Notifier.UpdateListAsync();

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

        private async Task HandlePreviousPage()
        {
            await Notifier.UpdateListAsync(page: previousPage);
            DisableOrEnablePreviousPageLink(Notifier.PreviousPage);
            DisableOrEnableNextPageLink(Notifier.NextPage);
        }

        private async Task HandleNextPage()
        {
            await Notifier.UpdateListAsync(page: nextPage);
            DisableOrEnablePreviousPageLink(Notifier.PreviousPage);
            DisableOrEnableNextPageLink(Notifier.NextPage);
        }
    }
}
