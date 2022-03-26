namespace ResourceIdea.Core.Handlers.Clients;

public class ClientsHandler : IClientsHandler
{
    private readonly ResourceIdeaDBContext _dbContext;

    public ClientsHandler(ResourceIdeaDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<ClientViewModel>> GetPaginatedResultAsync(string? subscriptionCode,
        int currentPage,
        int pageSize = 10,
        string? search = null)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode);

        var data = await GetDataAsync(subscriptionCode, search);
        return data.OrderBy(d => d.Name)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public async Task<int> GetCountAsync(string? subscriptionCode, string? search)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode);

        var data = await GetDataAsync(subscriptionCode, search);
        return data.Count;
    }

    /// <summary>
    /// Get the client by Id.
    /// </summary>
    /// <param name="subscriptionCode">Subscription code.</param>
    /// <param name="clientId">Client Id.</param>
    /// <returns>Tuple with flag indicating success and client object, null if not found</returns>
    /// <exception cref="ArgumentNullException">Throws ArgumentNullException if subscriptionCode is null.</exception>
    /// <exception cref="ArgumentNullException">Throws ArgumentNullException if clientId is null.</exception>
    public async Task<ClientViewModel?> GetClientByIdAsync(string? subscriptionCode, string? clientId)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode);
        ArgumentNullException.ThrowIfNull(clientId);

        var clientQuery = await _dbContext.Clients.SingleOrDefaultAsync(c => c.ClientId == clientId);
        if (clientQuery is not null)
        {
            return new ClientViewModel(
                clientQuery.ClientId,
                clientQuery.Name,
                clientQuery.Address,
                clientQuery.Industry
            );
        }

        return null;
    }

    private async Task<IList<ClientViewModel>> GetDataAsync(string? subscriptionCode, string? search)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode);

        var data = _dbContext.Clients
            .Where(c => c.CompanyCode == subscriptionCode);

        if (search != null)
        {
            data = data.Where(d => d.Name.Contains(search) || d.Address!.Contains(search));
        }

        return await data.Select(c => new ClientViewModel(
                c.ClientId,
                c.Name,
                c.Address,
                c.Industry))
            .ToListAsync();
    }
}