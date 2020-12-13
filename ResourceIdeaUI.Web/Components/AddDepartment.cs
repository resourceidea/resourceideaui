using Microsoft.AspNetCore.Components;
using ResourceIdeaUI.Shared.Models;
using ResourceIdeaUI.Web.Services;

namespace ResourceIdeaUI.Web.Components
{
    public partial class AddDepartment
    {
        private string DepartmentName { get; set; }

        [Inject]
        public IDepartmentService DepartmentService { get; set; }

        [Inject]
        public NewDepartmentNotifierService Notifier { get; set; }

        [Inject]
        public ToastService ToastService { get; set; }

        private async void HandleValidSubmit()
        {
            if (!string.IsNullOrEmpty(DepartmentName))
            {
                var department = new Department { Name = DepartmentName };
                DepartmentName = null;
                await DepartmentService.AddDepartmentAsync(department);
                await Notifier.UpdateListAsync();
                ToastService.ShowToast($"{department.Name} added successfully", ToastLevel.Success);
            }
        }
    }
}
