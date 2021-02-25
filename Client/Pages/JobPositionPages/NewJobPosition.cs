using Client.Models.DataModels;
using Client.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Pages.JobPositionPages
{
    public partial class NewJobPosition
    {
        private string Title { get; set; }

        [Parameter]
        public string DepartmentId { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IJobPositionService JobPositionService { get; set; }

        private async void Save()
        {
            JobPosition newJobPosition = new JobPosition();
            if (Guid.TryParse(DepartmentId, out Guid id))
            {
                newJobPosition = await JobPositionService.AddJobPosition(new JobPosition
                {
                    Department = id,
                    Title = Title
                });
            }
            NavigationManager.NavigateTo($"/departments/{newJobPosition.Department}");
        }
    }
}
