using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.Application.Abstractions;
using PayrollSystem.Domain.Entities;
using PayrollSystem.Infrastructure.Persistence;

namespace PayrollSystem.Web.Controllers;

public sealed class EmployeesController : Controller
{
    private readonly AppDbContext _db;
    private readonly IRepository<Employee> _employees;

    public EmployeesController(AppDbContext db, IRepository<Employee> employees)
    {
        _db = db;
        _employees = employees;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var items = await _db.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .OrderBy(e => e.EmployeeNo)
            .ToListAsync(cancellationToken);

        return View(items);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await LoadDepartmentsAsync(cancellationToken);
        return View(new Employee { HireDate = DateOnly.FromDateTime(DateTime.Today) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadDepartmentsAsync(cancellationToken);
            return View(model);
        }

        await _employees.AddAsync(model, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var item = await _employees.GetByIdAsync(id, cancellationToken);
        if (item is null) return NotFound();

        await LoadDepartmentsAsync(cancellationToken);
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Employee model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();

        if (!ModelState.IsValid)
        {
            await LoadDepartmentsAsync(cancellationToken);
            return View(model);
        }

        await _employees.UpdateAsync(model, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var item = await _db.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (item is null) return NotFound();
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var item = await _employees.GetByIdAsync(id, cancellationToken);
        if (item is null) return NotFound();

        await _employees.DeleteAsync(item, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadDepartmentsAsync(CancellationToken cancellationToken)
    {
        var departments = await _db.Departments
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .ToListAsync(cancellationToken);

        ViewBag.DepartmentId = new SelectList(departments, nameof(Department.Id), nameof(Department.Name));
    }
}
