using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.Application.Abstractions;
using PayrollSystem.Domain.Entities;
using PayrollSystem.Infrastructure.Persistence;
using PayrollSystem.Web.ViewModels;

namespace PayrollSystem.Web.Controllers;

public sealed class LeaveController : Controller
{
    private readonly AppDbContext _db;
    private readonly IRepository<LeaveType> _leaveTypes;
    private readonly ILeaveService _leave;

    public LeaveController(AppDbContext db, IRepository<LeaveType> leaveTypes, ILeaveService leave)
    {
        _db = db;
        _leaveTypes = leaveTypes;
        _leave = leave;
    }

    public async Task<IActionResult> Types(CancellationToken cancellationToken)
    {
        var items = await _leaveTypes.ListAsync(cancellationToken);
        return View(items.OrderBy(t => t.Name).ToList());
    }

    public IActionResult CreateType() => View(new LeaveType());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateType(LeaveType model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);
        await _leaveTypes.AddAsync(model, cancellationToken);
        return RedirectToAction(nameof(Types));
    }

    public async Task<IActionResult> Requests(CancellationToken cancellationToken)
    {
        var items = await _db.LeaveRequests
            .AsNoTracking()
            .Include(r => r.Employee)
            .Include(r => r.LeaveType)
            .OrderByDescending(r => r.RequestedAtUtc)
            .Take(200)
            .ToListAsync(cancellationToken);

        return View(items);
    }

    public async Task<IActionResult> Request(CancellationToken cancellationToken)
    {
        await LoadEmployeesAndTypesAsync(cancellationToken);
        return View(new LeaveRequestCreateVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Request(LeaveRequestCreateVm vm, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadEmployeesAndTypesAsync(cancellationToken);
            return View(vm);
        }

        await _leave.RequestAsync(vm.EmployeeId, vm.LeaveTypeId, vm.StartDate, vm.EndDate, vm.Reason, cancellationToken);
        return RedirectToAction(nameof(Requests));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id, CancellationToken cancellationToken)
    {
        await _leave.ApproveAsync(id, decidedBy: "HR", cancellationToken);
        return RedirectToAction(nameof(Requests));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id, CancellationToken cancellationToken)
    {
        await _leave.RejectAsync(id, decidedBy: "HR", cancellationToken);
        return RedirectToAction(nameof(Requests));
    }

    private async Task LoadEmployeesAndTypesAsync(CancellationToken cancellationToken)
    {
        var employees = await _db.Employees
            .AsNoTracking()
            .Where(e => e.IsActive)
            .OrderBy(e => e.EmployeeNo)
            .ToListAsync(cancellationToken);

        var types = await _db.LeaveTypes
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);

        ViewBag.EmployeeId = new SelectList(employees, nameof(Employee.Id), nameof(Employee.EmployeeNo));
        ViewBag.LeaveTypeId = new SelectList(types, nameof(LeaveType.Id), nameof(LeaveType.Name));
    }
}

