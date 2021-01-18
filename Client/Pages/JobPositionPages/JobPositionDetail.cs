using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Pages.JobPositionPages
{
    public partial class JobPositionDetail
    {
        [Parameter]
        public string JobPositionId { get; set; }

        [Parameter]
        public string DepartmentId { get; set; }
    }
}
