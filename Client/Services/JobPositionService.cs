using Client.Models.DataModels;
using Client.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface IJobPositionService
    {
        Task<List<JobPosition>> GetJobPositions(string page = null);
        Task<JobPositionResponse> AddJobPosition(JobPosition jobPosition);
        Task<JobPosition> GetJobPositionById(Guid id);
        Task<JobPosition> UpdateJobPosition(JobPosition jobPosition);
    }

    public class JobPositionService : IJobPositionService
    {
        private readonly IHttpService httpService;

        public JobPositionService(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<JobPositionResponse> AddJobPosition(JobPosition jobPosition)
        {
            return await httpService.Post<JobPositionResponse>($"/api/jobpositions/", jobPosition);
        }

        public async Task<JobPosition> GetJobPositionById(Guid id)
        {
            return await httpService.Get<JobPosition>($"/api/jobpositions/{id}");
        }

        public async Task<List<JobPosition>> GetJobPositions(string page = null)
        {
            return await httpService.Get<List<JobPosition>>($"/api/jobpositions/");
        }

        public async Task<JobPosition> UpdateJobPosition(JobPosition jobPosition)
        {
            return await httpService.Put<JobPosition>($"/api/jobpositions/{jobPosition.Id}", jobPosition);
        }
    }
}
