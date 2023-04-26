using Microsoft.EntityFrameworkCore;

using ResourceIdea.Models;

namespace ResourceIdea.Web.Core.Handlers.Tasks
{
    public class TaskHandler : ITaskHandler
    {
        private readonly ResourceIdeaDBContext dbContext;

        /// <summary>
        /// Initializes <see cref="TaskHandler"/>
        /// </summary>
        /// <param name="dBContext"></param>
        public TaskHandler(ResourceIdeaDBContext dBContext)
        {
            this.dbContext = dBContext;
        }

        /// <inheritdoc />
        public Task<string> AddAsync(string? subscriptionCode, TaskViewModel engagement)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<int> GetCountAsync(string? subscriptionCode, Dictionary<string, string>? filters, string? search)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<TaskViewModel?> GetTaskByIdAsync(string subscriptionCode, string? taskId)
        {
            if (subscriptionCode is null)
            {
                throw new MissingSubscriptionCodeException();
            }

            ArgumentNullException.ThrowIfNull(taskId);
            TaskViewModel? taskView = null;
            var queryResult = await CommonEngagementTasksQuery(subscriptionCode).FirstOrDefaultAsync(task => task.Id == taskId);
            if (queryResult is not null)
            {
                taskView = new TaskViewModel
                {
                    Id = queryResult.Id,
                    ClientId = queryResult.Engagement.ClientId,
                    Client = queryResult.Engagement.Client.Name,
                    EngagementId = queryResult.EngagementId,
                    Engagement = queryResult.Engagement.Name,
                    Description = queryResult.Description,
                    Status = queryResult.Status,
                    Color = queryResult.Color,
                    Manager = queryResult.Manager,
                    Partner = queryResult.Partner
                };
            }

            return taskView;
        }

        /// <inheritdoc/>
        public async Task<IList<TaskViewModel>> GetPaginatedListByEngagementAsync(
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

            var engagementTasks = GetEngagementTasks(subscriptionCode, engagementId, search).ApplyCustomFilters(filters);
            return await engagementTasks.OrderBy(d => d.Description)
                                        .Skip((currentPage - 1) * pageSize)
                                        .Take(pageSize)
                                        .Select(task => new TaskViewModel
                                        {
                                            Id = task.Id,
                                            Client = task.Engagement.Client.Name,
                                            ClientId = task.Engagement.ClientId,
                                            Color = task.Color,
                                            Description = task.Description,
                                            Engagement = task.Engagement.Name,
                                            EngagementId = task.EngagementId,
                                            Manager = task.Manager,
                                            Partner = task.Partner,
                                            Status = task.Status
                                        })
                                        .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(string subscriptionCode, TaskViewModel input)
        {
            ArgumentNullException.ThrowIfNull(input.EngagementId, nameof(input.EngagementId));
            var engagementForUpdate = await dbContext.EngagementTasks
                                                     .FirstOrDefaultAsync(task => task.Engagement.Client.CompanyCode == subscriptionCode
                                                                               && task.EngagementId == input.EngagementId
                                                                               && task.Engagement.ClientId == input.ClientId);

            if (engagementForUpdate is not null)
            {
                engagementForUpdate.Color = input.Color ?? string.Empty;
                engagementForUpdate.Description = input.Description ?? "NA";

                await dbContext.SaveChangesAsync();
            }
        }

        private IQueryable<EngagementTask> GetEngagementTasks(string? subscriptionCode, string? engagementId, string? search)
        {

            ArgumentNullException.ThrowIfNull(engagementId);

            var tasks = CommonEngagementTasksQuery(subscriptionCode).Where(task => task.EngagementId == engagementId);

            if (search != null)
            {
                tasks = tasks.Where(d => d.Description.Contains(search));
            }

            return tasks;
        }

        private IQueryable<EngagementTask> CommonEngagementTasksQuery(string? subscriptionCode)
        {
            if (subscriptionCode is null)
            {
                throw new MissingSubscriptionCodeException();
            }

            return dbContext.EngagementTasks
                            .Include(task => task.Engagement)
                            .Include(task => task.Engagement.Client)
                            .Where(task => task.Engagement.Client.CompanyCode == subscriptionCode);
        }

        /// <inheritdoc />
        public Task<IList<TaskViewModel>> GetPaginatedListByClientAsync(string? subscriptionCode, string? clientId, int currentPage, int pageSize = 10, Dictionary<string, string>? filters = null, string? search = null)
        {
            throw new NotImplementedException();
        }
    }
}
