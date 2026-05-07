using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.Application.Abstractions;
using PayrollSystem.Infrastructure.Persistence;
using PayrollSystem.Web.ViewModels;

namespace PayrollSystem.Web.Controllers;

public sealed class PayrollController : Controller
{
    private readonly AppDbContext _db;
    private readonly IPayrollService _payroll;

    public PayrollController(AppDbContext db, IPayrollService payroll)
    {
        _db = db;
        _payroll = payroll;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var runs = await _db.PayrollRuns
            .AsNoTracking()
            .Include(r => r.PayPeriod)
            .Include(r => r.Lines)
            .OrderByDescending(r => r.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return View(runs);
    }

    public IActionResult Create() => View(new PayrollRunCreateVm());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PayrollRunCreateVm vm, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(vm);

        var run = await _payroll.CreateRunAsync(vm.StartDate, vm.EndDate, cancellationToken);
        return RedirectToAction(nameof(Details), new { id = run.Id });
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var run = await _db.PayrollRuns
            .AsNoTracking()
            .Include(r => r.PayPeriod)
            .Include(r => r.Lines)
                .ThenInclude(l => l.Employee)
                    .ThenInclude(e => e!.Department)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (run is null) return NotFound();

        return View(run);
    }
}
