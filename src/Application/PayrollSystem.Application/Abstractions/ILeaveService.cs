using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Abstractions;

public interface ILeaveService
{
    Task<LeaveRequest> RequestAsync(
        int employeeId,
        int leaveTypeId,
        DateOnly startDate,
        DateOnly endDate,
        string? reason,
        CancellationToken cancellationToken = default);

    Task ApproveAsync(int requestId, string decidedBy, CancellationToken cancellationToken = default);
    Task RejectAsync(int requestId, string decidedBy, CancellationToken cancellationToken = default);
}

