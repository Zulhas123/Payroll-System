using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.Application.Abstractions;
using PayrollSystem.Domain.Entities;
using PayrollSystem.Infrastructure.Persistence;
using PayrollSystem.Web.ViewModels;

namespace PayrollSystem.Web.Controllers;

public sealed class AttendanceController : Controller
{
    private readonly AppDbContext _db;
    private readonly IAttendanceService _attendance;

    public AttendanceController(AppDbContext db, IAttendanceService attendance)
    {
        _db = db;
        _attendance = attendance;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var items = await _db.AttendanceRecords
            .AsNoTracking()
            .Include(a => a.Employee)
                .ThenInclude(e => e!.Department)
            .OrderByDescending(a => a.Date)
            .ThenBy(a => a.Employee!.EmployeeNo)
            .Take(200)
            .ToListAsync(cancellationToken);

        return View(items);
    }

    public async Task<IActionResult> Upsert(CancellationToken cancellationToken)
    {
        await LoadEmployeesAsync(cancellationToken);
        return View(new AttendanceUpsertVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(AttendanceUpsertVm vm, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadEmployeesAsync(cancellationToken);
            return View(vm);
        }

        await _attendance.UpsertManualAsync(vm.EmployeeId, vm.Date, vm.CheckIn, vm.CheckOut, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var item = await _db.AttendanceRecords
            .AsNoTracking()
            .Include(a => a.Employee)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (item is null) return NotFound();
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var item = await _db.AttendanceRecords.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (item is null) return NotFound();

        _db.AttendanceRecords.Remove(item);
        await _db.SaveChangesAsync(cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadEmployeesAsync(CancellationToken cancellationToken)
    {
        var employees = await _db.Employees
            .AsNoTracking()
            .Where(e => e.IsActive)
            .OrderBy(e => e.EmployeeNo)
            .ToListAsync(cancellationToken);

        ViewBag.EmployeeId = new SelectList(employees, nameof(Employee.Id), nameof(Employee.EmployeeNo));
    }
}

