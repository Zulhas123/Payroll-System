using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Abstractions;

public interface IAttendanceService
{
    Task<AttendanceRecord> UpsertManualAsync(
        int employeeId,
        DateOnly date,
        TimeOnly? checkIn,
        TimeOnly? checkOut,
        CancellationToken cancellationToken = default);
}

