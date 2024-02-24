using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using EastSeat.ResourceIdea.Domain.Entities;

using static Constants;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Specifications;

public static class EngagementSpecifications
{
    public static Expression<Func<Engagement, bool>> HasNameContaining(string searchKeyword) =>
        engagement => engagement.Name.Contains(searchKeyword);

    public static Expression<Func<Engagement, bool>> HasDescriptionContaining(string searchKeyword) =>
        engagement => engagement.Description.Contains(searchKeyword);

    public static Expression<Func<Engagement, bool>> StartsAfter(DateTime dateTime) =>
        engagement => engagement.StartDate > dateTime;

    public static Expression<Func<Engagement, bool>> EndsBefore(DateTime dateTime) =>
        engagement => engagement.EndDate < dateTime;

    public static Expression<Func<Engagement, bool>> StartsOn(DateTime dateTime) =>
        engagement => engagement.StartDate == dateTime;

    public static Expression<Func<Engagement, bool>> EndsOn(DateTime dateTime) =>
        engagement => engagement.EndDate == dateTime;

    public static Expression<Func<Engagement, bool>> HasStatus(EngagementStatus status) =>
        engagement => engagement.Status == status;

    public static Expression<Func<Engagement, bool>> HasClientId(Guid clientId) =>
        engagement => engagement.ClientId == clientId;
}
