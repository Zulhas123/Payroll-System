using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Abstractions;

public interface IPayrollService
{
    Task<PayrollRun> CreateRunAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
}
