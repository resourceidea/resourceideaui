﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ResourceIdeaUI.Shared.Models;

namespace ResourceIdeaUI.Web.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetDepartmentsAsync();
        Task<Department> AddDepartmentAsync(Department department);
    }

    public class DepartmentService : IDepartmentService
    {
        private IHttpService _httpService;

        public DepartmentService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            return await _httpService.Get<IEnumerable<Department>>("/api/v0.1/departments/");
        }

        public async Task<Department> AddDepartmentAsync(Department department)
        {
            return await _httpService.Post<Department>("/api/v0.1/departments/", department);
        }
    }
}
