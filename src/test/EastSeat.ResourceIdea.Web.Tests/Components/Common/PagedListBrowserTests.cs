using Bunit;

using EastSeat.ResourceIdea.Web.Components.Common;

using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Tests.Components.Common;

public class PagedListBrowserTests : TestContext
{
    [Fact]
    [Trait("Component", "Common")]
    public void PagedListBrowserRendersCorrectly()
    {
        // Arrange
        var cut = RenderComponent<PagedListBrowser>(parameters => parameters
            .Add(p => p.HasNextPage, true)
            .Add(p => p.HasPreviousPage, false)
            .Add(p => p.OnNext, EventCallback.Factory.Create(this, () => { }))
            .Add(p => p.OnPrevious, EventCallback.Factory.Create(this, () => { })));

        // Act - trigger the 'Next' button
        cut.Find("button.btn-outline-primary").Click();

        // Assert
        Assert.False(cut.FindAll("button.btn-outline-primary")[1].HasAttribute("disabled"));
        Assert.True(cut.FindAll("button.btn-outline-primary")[0].HasAttribute("disabled"));
    }
}
