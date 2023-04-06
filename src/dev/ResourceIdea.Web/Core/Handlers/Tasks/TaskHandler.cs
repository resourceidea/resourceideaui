using Microsoft.EntityFrameworkCore;

using ResourceIdea.Models;

namespace ResourceIdea.Web.Core.Handlers.Tasks
{
    public class TaskHandler : ITaskHandler
    {
        private readonly ResourceIdeaDBContext dbContext;

        public TaskHandler(ResourceIdeaDBContext dBContext)
        {
            this.dbContext = dBContext;
        }

        /// <summary>
        /// Add task.
        /// </summary>
        /// <param name="subscriptionCode"></param>
        /// <param name="engagement"></param>
        /// <returns></returns>
        public Task<string> AddAsync(string? subscriptionCode, TaskViewModel engagement)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get count of tasks in query.
        /// </summary>
        /// <param name="subscriptionCode"></param>
        /// <param name="filters"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public Task<int> GetCountAsync(string? subscriptionCode, Dictionary<string, string>? filters, string? search)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<TaskViewModel?> GetTaskById(string? subscriptionCode, string? taskId)
        {
            if (subscriptionCode is null)
            {
                throw new MissingSubscriptionCodeException();
            }

            ArgumentNullException.ThrowIfNull(taskId);
            TaskViewModel? taskView = null;
            var taskQuery = await dbContext.Tasks.SingleOrDefaultAsync(task => task.Engagement.Client.CompanyCode == subscriptionCode
                                                                       && task.Id == taskId);
            if (taskQuery is not null)
            {
                taskView = new TaskViewModel(
                    TaskId: taskQuery.Id,
                    EngagementId: taskQuery.EngagementId,
                    Description: taskQuery.Description,
                    Status: taskQuery.Status,
                    Color: taskQuery.Color,
                    Manager: taskQuery.Manager,
                    Partner: taskQuery.Partner
                );
            }

            return taskView;
        }

        /// <inheritdoc/>
        public async Task<IList<TaskViewModel>> GetPaginatedListAsync(
            string? subscriptionCode,
            string? engagementId,
            int currentPage,
            int pageSize = 10,
            Dictionary<string, string>? filters = null,
            string? search = null)
        {
            if (subscriptionCode is null)
            {
                throw new MissingSubscriptionCodeException();
            }

            ArgumentNullException.ThrowIfNull(engagementId);

            var data = await GetDataAsync(subscriptionCode, engagementId, search);
            return data.OrderBy(d => d.Description)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        /// <inheritdoc/>
        public System.Threading.Tasks.Task UpdateAsync(string? subscriptionCode, TaskViewModel input)
        {
            throw new NotImplementedException();
        }

        private async Task<IList<TaskViewModel>> GetDataAsync(string? subscriptionCode, string? engagementId, string? search)
        {
            if (subscriptionCode is null)
            {
                throw new MissingSubscriptionCodeException();
            }

            ArgumentNullException.ThrowIfNull(engagementId);

            var data = dbContext.Tasks
                .Where(task => task.Engagement.Client.CompanyCode == subscriptionCode
                                && task.EngagementId == engagementId);

            if (search != null)
            {
                data = data.Where(d => d.Description.Contains(search));
            }

            return await data.Select(task => new TaskViewModel(
                    task.Id ?? string.Empty,
                    task.EngagementId ?? string.Empty,
                    task.Description ?? string.Empty,
                    task.Status ?? string.Empty,
                    task.Color ?? string.Empty,
                    task.Manager ?? string.Empty,
                    task.Partner ?? string.Empty))
                .ToListAsync();
        }
    }
}
