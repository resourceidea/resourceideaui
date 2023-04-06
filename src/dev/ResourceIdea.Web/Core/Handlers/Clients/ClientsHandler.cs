namespace ResourceIdea.Web.Core.Handlers.Clients;

public class ClientsHandler : IClientsHandler
{
    private readonly ResourceIdeaDBContext _dbContext;

    /// <summary>
    /// Initializes <see cref="ClientsHandler"/>
    /// </summary>
    /// <param name="dbContext">Database context.</param>
    public ClientsHandler(ResourceIdeaDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IList<ClientViewModel>> GetPaginatedResultAsync(
        string? subscriptionCode,
        int currentPage,
        int pageSize = 10,
        string? search = null)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

        var data = await GetDataAsync(subscriptionCode, search);
        return data.OrderBy(d => d.Name)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    /// <inheritdoc />
    public async Task<int> GetCountAsync(string? subscriptionCode, string? search)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

        var data = await GetDataAsync(subscriptionCode, search);
        return data.Count;
    }
    
    /// <inheritdoc />
    public async Task<ClientViewModel?> GetClientByIdAsync(
        string? subscriptionCode, 
        string? clientId)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

        ArgumentNullException.ThrowIfNull(clientId);

        ClientViewModel? result = null;

        var clientQuery = await _dbContext.Clients.SingleOrDefaultAsync(c => c.ClientId == clientId);
        if (clientQuery is not null)
        {
            result = new ClientViewModel(
                clientQuery.ClientId,
                clientQuery.Name,
                clientQuery.Address,
                clientQuery.Industry,
                clientQuery.Active
            );
        }

        return result;
    }

    /// <inheritdoc />
    public async System.Threading.Tasks.Task UpdateAsync(string? subscriptionCode, ClientViewModel input)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

        var clientForUpdate = await _dbContext.Clients
            .SingleOrDefaultAsync(c => c.CompanyCode == subscriptionCode && c.ClientId == input.ClientId);

        if (clientForUpdate is not null)
        {
            clientForUpdate.Address = input.Address;
            clientForUpdate.Industry = input.Industry;
            clientForUpdate.Name = input.Name??"N/A";

            await _dbContext.SaveChangesAsync();
        }

        ArgumentNullException.ThrowIfNull(input.ClientId, nameof(input.ClientId));
    }

    /// <inheritdoc />
    public async Task<string> AddAsync(string? subscriptionCode, ClientViewModel client)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

        ArgumentNullException.ThrowIfNull(client.ClientId, nameof(client.ClientId));
        ArgumentNullException.ThrowIfNull(client.Name, nameof(client.Name));

        var result = await _dbContext.Clients.AddAsync(
            new Client()
            {
                Name = client.Name,
                Address = client.Address,
                Industry = client.Industry,
                CompanyCode = subscriptionCode,
                ClientId = client.ClientId,
                Active = client.Active
            });
        await _dbContext.SaveChangesAsync();

        return result.Entity.ClientId;
    }

    private async Task<IList<ClientViewModel>> GetDataAsync(string? subscriptionCode, string? search)
    {
        if (subscriptionCode is null)
        {
            throw new MissingSubscriptionCodeException();
        }

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
                c.Industry,
                c.Active))
            .ToListAsync();
    }
}