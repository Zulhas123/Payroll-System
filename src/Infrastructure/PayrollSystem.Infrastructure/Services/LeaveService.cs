using Microsoft.EntityFrameworkCore;
using PayrollSystem.Application.Abstractions;
using PayrollSystem.Domain.Entities;
using PayrollSystem.Infrastructure.Persistence;

namespace PayrollSystem.Infrastructure.Services;

public sealed class LeaveService : ILeaveService
{
    private readonly AppDbContext _db;

    public LeaveService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<LeaveRequest> RequestAsync(
        int employeeId,
        int leaveTypeId,
        DateOnly startDate,
        DateOnly endDate,
        string? reason,
        CancellationToken cancellationToken = default)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException("End date must be on or after start date.");
        }

        var typeExists = await _db.LeaveTypes.AnyAsync(t => t.Id == leaveTypeId, cancellationToken);
        if (!typeExists) throw new InvalidOperationException("Leave type not found.");

        var request = new LeaveRequest
        {
            EmployeeId = employeeId,
            LeaveTypeId = leaveTypeId,
            StartDate = startDate,
            EndDate = endDate,
            Reason = reason,
            Status = LeaveStatus.Pending,
            RequestedAtUtc = DateTime.UtcNow
        };

        _db.LeaveRequests.Add(request);
        await _db.SaveChangesAsync(cancellationToken);
        return request;
    }

    public Task ApproveAsync(int requestId, string decidedBy, CancellationToken cancellationToken = default)
        => DecideAsync(requestId, LeaveStatus.Approved, decidedBy, cancellationToken);

    public Task RejectAsync(int requestId, string decidedBy, CancellationToken cancellationToken = default)
        => DecideAsync(requestId, LeaveStatus.Rejected, decidedBy, cancellationToken);

    private async Task DecideAsync(int requestId, LeaveStatus status, string decidedBy, CancellationToken cancellationToken)
    {
        var request = await _db.LeaveRequests.FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);
        if (request is null) throw new InvalidOperationException("Leave request not found.");

        request.Status = status;
        request.DecidedBy = decidedBy;
        request.DecidedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
    }
}

