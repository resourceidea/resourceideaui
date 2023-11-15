using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Subscription.Commands.CreateSubscription;

using FluentValidation.Results;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Employee.Commands.CreateEmployee
{
    /// <summary>
    /// Handles the command to create an employee.
    /// </summary>
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, CreateEmployeeCommandResponse>
    {
        private readonly IMapper mapper;
        private readonly IAsyncRepository<Domain.Entities.Employee> employeeRepository;

        public CreateEmployeeCommandHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Employee> employeeRepository)
        {
            this.mapper = mapper;
            this.employeeRepository = employeeRepository;
        }

        /// <inheritdoc />
        public async Task<CreateEmployeeCommandResponse> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            CreateEmployeeCommandResponse response = new ();

            CreateEmployeeCommandValidator commandValidator = new ();
            ValidationResult commandValidationResult = await commandValidator.ValidateAsync(request, cancellationToken);
            if (commandValidationResult.Errors.Count > 0)
            {
                response.Success = false;
                response.Errors = new List<string>();
                foreach (var error in commandValidationResult.Errors)
                {
                    response.Errors.Add(error.ErrorMessage);
                }
            }

            if (response.Success)
            {
                var employee = new Domain.Entities.Employee
                {
                    SubscriptionId = request.SubscriptionId,
                    JobPositionId = request.JobPositionId,
                    UserId = request.UserId,
                    Id = Guid.NewGuid()
                };
                employee = await employeeRepository.AddAsync(employee);
                response.Content = mapper.Map<CreateEmployeeViewModel>(employee);
            }

            return response;
        }
    }
}
