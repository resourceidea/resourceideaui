using Microsoft.EntityFrameworkCore;
using EastSeat.ResourceIdea.Server.Data;
using EastSeat.ResourceIdea.Shared.Models;
using Xunit;

namespace ServerTests;

public class ClientCrudTests
{
    [Fact]
    public async Task CanCreateAndRetrieveClient()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var db = new ApplicationDbContext(options);
        var client = new Client { Name = "Test Client", Code = "TC1" };
        db.Clients.Add(client);
        await db.SaveChangesAsync();
        var loaded = await db.Clients.FirstAsync(c => c.Code == "TC1");
        Assert.Equal(client.Name, loaded.Name);
    }
}
