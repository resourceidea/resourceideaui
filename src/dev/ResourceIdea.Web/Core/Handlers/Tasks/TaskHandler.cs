namespace ResourceIdea.Web.Core.Handlers.Tasks
{
    public class TaskHandler : ITaskHandler
    {
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

        /// <summary>
        /// Get task by id.
        /// </summary>
        /// <param name="subscriptionCode"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Task<TaskViewModel?> GetEngagementByIdAsync(string? subscriptionCode, string? taskId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get paginated list of tasks.
        /// </summary>
        /// <param name="subscriptionCode"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="filters"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public Task<IList<TaskViewModel>> GetPaginatedListAsync(
            string? subscriptionCode,
            int currentPage,
            int pageSize = 10,
            Dictionary<string, string>? filters = null,
            string? search = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update task
        /// </summary>
        /// <param name="subscriptionCode"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task UpdateAsync(string? subscriptionCode, TaskViewModel input)
        {
            throw new NotImplementedException();
        }
    }
}
