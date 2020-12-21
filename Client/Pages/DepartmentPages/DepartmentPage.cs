using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Pages.DepartmentPages
{
    public partial class DepartmentPage
    {
        [Parameter]
        public string Id { get; set; }
    }
}
