using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Pages.JobPositionPages
{
    public partial class NewJobPosition
    {
        [Parameter]
        public string DepartmentId { get; set; }
    }
}
