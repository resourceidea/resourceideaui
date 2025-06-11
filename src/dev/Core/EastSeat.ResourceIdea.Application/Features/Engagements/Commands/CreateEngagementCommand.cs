using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Commands;

/// <summary>
/// Represents a command to create a new engagement.
/// </summary>
public sealed class CreateEngagementCommand : BaseRequest<EngagementModel>
{
    /// <summary>
    /// Gets or sets the client ID.
    /// </summary>
    public ClientId ClientId { get; set; }

    /// <summary>
    /// Gets or sets the title of the engagement.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the engagement.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the due date of the engagement.
    /// </summary>
    public DateTimeOffset? DueDate { get; set; }

    /// <summary>
    /// Gets or sets the status of the engagement.
    /// </summary>
    public EngagementStatus Status { get; set; } = EngagementStatus.NotStarted;

    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            Title.ValidateRequired(nameof(Title)),
            Description.ValidateRequired(nameof(Description)),
            ClientId.ValidateRequired(),
            TenantId.ValidateRequired()
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}