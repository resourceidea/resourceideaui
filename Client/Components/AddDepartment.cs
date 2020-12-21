using Client.Models.DataModels;
using Client.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Components
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
                await DepartmentService.AddDepartment(department);
                await Notifier.UpdateListAsync();
                ToastService.ShowToast($"{department.Name} added successfully", ToastLevel.Success);
            }
        }
    }
}
