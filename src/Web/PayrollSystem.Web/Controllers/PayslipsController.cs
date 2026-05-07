using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.Domain.Entities;
using PayrollSystem.Infrastructure.Persistence;

namespace PayrollSystem.Web.Controllers;

public sealed class PayslipsController : Controller
{
    private readonly AppDbContext _db;

    public PayslipsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var employees = await _db.Employees
            .AsNoTracking()
            .Where(e => e.IsActive)
            .OrderBy(e => e.EmployeeNo)
            .ToListAsync(cancellationToken);

        ViewBag.EmployeeId = new SelectList(employees, nameof(Employee.Id), nameof(Employee.EmployeeNo));
        return View();
    }

    public async Task<IActionResult> ViewPayslip(int employeeId, int runId, CancellationToken cancellationToken)
    {
        var run = await _db.PayrollRuns
            .AsNoTracking()
            .Include(r => r.PayPeriod)
            .Include(r => r.Lines)
                .ThenInclude(l => l.Employee)
                    .ThenInclude(e => e!.Department)
            .FirstOrDefaultAsync(r => r.Id == runId, cancellationToken);

        if (run is null) return NotFound();

        var line = run.Lines.FirstOrDefault(l => l.EmployeeId == employeeId);
        if (line is null) return NotFound();

        ViewBag.Run = run;
        return View(line);
    }
}

