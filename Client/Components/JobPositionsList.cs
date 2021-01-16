using Client.Models.DataModels;
using Client.Models.ResponseModels;
using Client.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Components
{
    public partial class JobPositionsList
    {
        private List<JobPositionResponse> jobPositions = new List<JobPositionResponse>();

        [Inject]
        public IJobPositionService JobPositionService { get; set; }

        [Parameter]
        public Guid? DepartmentId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            jobPositions = await JobPositionService.GetJobPositions();
        }
    }
}
