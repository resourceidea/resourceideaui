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

    private async Task<IList<ClientViewModel>> GetDataAsync(string? subscriptionCode, string? search)
    {
        ArgumentNullException.ThrowIfNull(subscriptionCode);
        
        var data = _dbContext.Clients
            .Where(c => c.CompanyCode == subscriptionCode);
        
        if (search != null)
        {
            data = data.Where(d => d.Name.Contains(search) || d.Address!.Contains(search));
        }
        
        return await data.Select(c => new ClientViewModel(c.ClientId, c.Name, c.Address, c.Industry))
                         .ToListAsync();
    }
}