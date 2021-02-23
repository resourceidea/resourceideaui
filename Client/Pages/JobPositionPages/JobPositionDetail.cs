using Client.Models.DataModels;
using Client.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Pages.JobPositionPages
{
    public partial class JobPositionDetail
    {
        private JobPosition jobPosition = new JobPosition();

        [Parameter]
        public string JobPositionId { get; set; }

        [Parameter]
        public string DepartmentId { get; set; }

        [Inject]
        public IJobPositionService JobPositionService { get; set; }

        [Inject]
        public ToastService ToastService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Guid.TryParse(JobPositionId, out Guid id))
            {
                jobPosition = await JobPositionService.GetJobPositionById(id);
            }
        }

        private async void SaveChanges()
        {
            if(!string.IsNullOrWhiteSpace(jobPosition.Title))
            {
                await JobPositionService.UpdateJobPosition(jobPosition);
                ToastService.ShowToast("Changes saved successfully", ToastLevel.Success);
            }
        }
    }
}
