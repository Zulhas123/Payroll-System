using Microsoft.EntityFrameworkCore;
using PayrollSystem.Application.Abstractions;
using PayrollSystem.Domain.Entities;
using PayrollSystem.Infrastructure.Persistence;

namespace PayrollSystem.Infrastructure.Services;

public sealed class PayrollService : IPayrollService
{
    private readonly AppDbContext _db;

    public PayrollService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PayrollRun> CreateRunAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException("End date must be on or after start date.");
        }

        var period = await _db.PayPeriods
            .OrderByDescending(p => p.Id)
            .FirstOrDefaultAsync(p => p.StartDate == startDate && p.EndDate == endDate, cancellationToken);

        if (period is null)
        {
            period = new PayPeriod { StartDate = startDate, EndDate = endDate };
            _db.PayPeriods.Add(period);
            await _db.SaveChangesAsync(cancellationToken);
        }

        var employees = await _db.Employees
            .AsNoTracking()
            .Where(e => e.IsActive)
            .OrderBy(e => e.EmployeeNo)
            .ToListAsync(cancellationToken);

        var run = new PayrollRun
        {
            PayPeriodId = period.Id,
            CreatedAtUtc = DateTime.UtcNow,
            Lines = employees.Select(e => new PayrollLine
            {
                EmployeeId = e.Id,
                Basic = e.BasicSalary,
                Allowances = 0m,
                Deductions = 0m,
            }).ToList()
        };

        _db.PayrollRuns.Add(run);
        await _db.SaveChangesAsync(cancellationToken);

        return run;
    }
}
