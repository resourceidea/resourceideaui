using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using ResourceIdeaUI.Shared.Models;
using ResourceIdeaUI.Web.Services;

namespace ResourceIdeaUI.Web.Pages.DepartmentPages
{
    public partial class DepartmentPage
    {
        [Parameter]
        public string Id { get; set; }
    }
}
