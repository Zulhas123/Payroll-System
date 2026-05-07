using Microsoft.EntityFrameworkCore;
using PayrollSystem.Application.Abstractions;
using PayrollSystem.Domain.Entities;
using PayrollSystem.Infrastructure.Persistence;

namespace PayrollSystem.Infrastructure.Services;

public sealed class AttendanceService : IAttendanceService
{
    private readonly AppDbContext _db;

    public AttendanceService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AttendanceRecord> UpsertManualAsync(
        int employeeId,
        DateOnly date,
        TimeOnly? checkIn,
        TimeOnly? checkOut,
        CancellationToken cancellationToken = default)
    {
        var existing = await _db.AttendanceRecords
            .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == date, cancellationToken);

        if (existing is null)
        {
            existing = new AttendanceRecord
            {
                EmployeeId = employeeId,
                Date = date,
                Source = AttendanceSource.Manual
            };
            _db.AttendanceRecords.Add(existing);
        }

        existing.CheckIn = checkIn;
        existing.CheckOut = checkOut;
        existing.Source = AttendanceSource.Manual;

        await _db.SaveChangesAsync(cancellationToken);
        return existing;
    }
}

