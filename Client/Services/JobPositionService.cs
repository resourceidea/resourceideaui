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
        Task<List<JobPositionResponse>> GetJobPositions(string page = null);
        Task<JobPositionResponse> AddJobPosition(JobPosition jobPosition);
        Task<JobPosition> GetJobPositionById(Guid id);
        Task<JobPosition> UpdateJobPosition(JobPosition jobPosition);
    }

    public class JobPositionService : IJobPositionService
    {
        private IHttpService httpService;

        public JobPositionService(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<JobPositionResponse> AddJobPosition(JobPosition jobPosition)
        {
            return await httpService.Post<JobPositionResponse>($"/api/jobpositions/", jobPosition);
        }

        public Task<JobPosition> GetJobPositionById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<JobPositionResponse>> GetJobPositions(string page = null)
        {
            return await httpService.Get<List<JobPositionResponse>>($"/api/jobpositions/");
        }

        public Task<JobPosition> UpdateJobPosition(JobPosition jobPosition)
        {
            throw new NotImplementedException();
        }
    }
}
