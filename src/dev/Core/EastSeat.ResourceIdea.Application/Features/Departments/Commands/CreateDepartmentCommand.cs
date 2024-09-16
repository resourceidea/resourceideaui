using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Departments.Models;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Commands;

public sealed class CreateDepartmentCommand :IRequest<ResourceIdeaResponse<NewDepartmentModel>>
{
    public required string DepartmentName { get; set; }
}
